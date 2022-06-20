using DataManager.Models;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace DataManager.MachineLearning;

public class MLPredictor : IMLPredictor
{
    private readonly MLContext _mlContext;

    private readonly ILogger<MLPredictor> _logger;

    public MLPredictor(ILogger<MLPredictor> logger)
    {
        _logger = logger;
        _mlContext = new MLContext(seed: 0);
    }
    void TestSinglePrediction(ITransformer model)
    {
        var predictionFunction = _mlContext.Model.CreatePredictionEngine<NMeasured, NPredicted>(model);
        var measuredPrediction = new NMeasured()
        {
            CreatedDate = DateTime.Now.AddDays(100).ToString(),
            Quantity = 60,
            N = 0 // To predict.
        };
        var prediction = predictionFunction.Predict(measuredPrediction);
        Console.WriteLine($"Predicted fare: {prediction.N:0.####}, actual fare: 15.5");

    }
    ITransformer Train(MLContext mlContext, IDataView dataView)
    {
        var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "N")
            .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "CreatedDateEncoded", inputColumnName: "CreatedDate"))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "QuantityEncoded", inputColumnName: "Quantity"))

            .Append(mlContext.Transforms.Concatenate("Features", "CreatedDateEncoded", "QuantityEncoded"))
            .Append(mlContext.Regression.Trainers.FastForest());
        var model = pipeline.Fit(dataView);
        return model;


    }
    void Evaluate(MLContext mlContext, ITransformer model, IDataView secondYearData)
    {
        var predictions = model.Transform(secondYearData);
        var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
        Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
        Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");

    }
    public void TrainAndPredict(string cleanDataPath, string modelPath)
    {
        //loading data from datasource. --> used as train data because is typically larger than testing data
        IDataView dataView = _mlContext.Data.LoadFromTextFile<MeasuredData>(cleanDataPath, hasHeader: true, separatorChar: ',');
        //train data
      //  IDataView firstYearData = _mlContext.Data.FilterRowsByColumn(dataView, "year", upperBound: 1);

        //testData
        IDataView testData = _mlContext.Data.FilterRowsByColumn(dataView, "year", lowerBound:0);

        var model = Train(_mlContext, dataView);


        Evaluate(_mlContext, model, testData);

        TestSinglePrediction(model);

    }
}

