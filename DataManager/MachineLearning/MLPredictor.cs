using DataManager.Models;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.MachineLearning
{
    public class MLPredictor : IMLPredictor
    {
        private readonly MLContext _mlContext;

        private readonly ILogger<MLPredictor> _logger;

        public MLPredictor(ILogger<MLPredictor> logger)
        {
            _logger = logger;
            _mlContext = new MLContext(seed:0);
        }


        public void TrainAndPredict(string cleanDataPath, string modelPath)
        {
            //loading data from datasource.
            IDataView dataView = _mlContext.Data.LoadFromTextFile<MeasuredData>(cleanDataPath, hasHeader: true, separatorChar: ','); 
            IDataView firstYearData = _mlContext.Data.FilterRowsByColumn(dataView, "Year", upperBound: 1);
            IDataView secondYearData = _mlContext.Data.FilterRowsByColumn(dataView, "Year", lowerBound: 1);

            var forecastingPipeline = _mlContext.Forecasting.ForecastBySsa(
            outputColumnName: "forecastedN",
            inputColumnName: "N",
            windowSize: 7,
            seriesLength: 30,
            trainSize: 365,
            horizon: 7,
            confidenceLevel: 0.95f,
            confidenceLowerBoundColumn: "LowerBoundRentals",
            confidenceUpperBoundColumn: "UpperBoundRentals");

            SsaForecastingTransformer forecaster = forecastingPipeline.Fit(firstYearData);

            Evaluate(secondYearData, forecaster, _mlContext);
            var forecastEngine = forecaster.CreateTimeSeriesEngine<MeasuredData, MeasuredDataPredictedOutputValues>(_mlContext);

            forecastEngine.CheckPoint(_mlContext, modelPath);
            //7== over 7 years
            Forecast(secondYearData, 7, forecastEngine, _mlContext);

        }

        public void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            IDataView predictions = model.Transform(testData);
            IEnumerable<float> actual =
         mlContext.Data.CreateEnumerable<MeasuredData>(testData, true)
        .Select(observed => observed.N);
            IEnumerable<float> forecast =
         mlContext.Data.CreateEnumerable<MeasuredDataPredictedOutputValues>(predictions, true)
        .Select(prediction => prediction.forecastedN[0]);

            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

        }


        void Forecast(IDataView testData, int horizon, TimeSeriesPredictionEngine<MeasuredData, MeasuredDataPredictedOutputValues> forecaster, MLContext mlContext)
        {
            MeasuredDataPredictedOutputValues forecast = forecaster.Predict();
            IEnumerable<string> forecastOutput =
    mlContext.Data.CreateEnumerable<MeasuredData>(testData, reuseRowObject: false)
        .Take(horizon)
        .Select((MeasuredData rental, int index) =>
        {
            string measureDate = rental.CreatedDate.ToShortDateString();
            float actualN = rental.N;
            float lowerEstimate = Math.Max(0, forecast.lowerBoundN[index]);
            float estimate = forecast.forecastedN[index];
            float upperEstimate = forecast.upperBoundN[index];
            return $"Date: {measureDate}\n" +
            $"Actual N: {actualN}\n" +
            $"Lower N: {lowerEstimate}\n" +
            $"Forecast: {estimate}\n" +
            $"Upper Estimate: {upperEstimate}\n";
        });
        }
    }
}
