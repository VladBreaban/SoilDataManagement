using DataManager.Models;

namespace SoilDataManagement.Worker;

public class Worker : IWorker
{
    private readonly ILogger<Worker> _logger;
    private readonly IDataManager _dataManager;
    private readonly IDataCleaner _dataCleaner;
    private readonly IElasticHelper _elasticHelper;
    private readonly IOptionsMonitor<PredictionFileOptionsMonitor> _optionsMonitor;
    public Worker(ILogger<Worker> logger, IDataManager dataManager, IDataCleaner dataCleaner, IElasticHelper elasticHelper, IOptionsMonitor<PredictionFileOptionsMonitor> optionsMonitor)
    {
        _logger = logger;
        _dataManager = dataManager;
        _dataCleaner = dataCleaner;
        _elasticHelper = elasticHelper;
        _optionsMonitor = optionsMonitor;
    }
    public async Task DoWork(CancellationToken cancelToken)
    {
        await Task.Yield();
        // to do--> make it configurable
        TimeSpan start = new TimeSpan(20, 0, 0); //10 o'clock
        TimeSpan end = new TimeSpan(20,10, 0); //12 o'clock
        while (!cancelToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.Now.TimeOfDay;
                if ((now > start) && (now < end))
                {
                    //match found --> get data from thinkspeak and send them to elastic server
                    _logger.LogInformation("Getting data for the current day...");
                    var allDataPath = await _dataManager.GetDataBetweenTimeInterval(DateTime.Now.AddDays(-1).ToString(), DateTime.Now.ToString());
                    if(!String.IsNullOrEmpty(allDataPath))
                    {
                        var cleanedData = await _dataCleaner.GetCleanData(allDataPath);

                        _logger.LogInformation("Sending data to elastic...");
                        cleanedData.ForEach(async x => { await _elasticHelper.IndexAsync(x, "soil-data"); });

                        _logger.LogInformation("Generating prediction files");

                        await GeneratePredicitonFiles(cleanedData);
                    } 
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occured while executing background task: {ex.Message}");
            }

        }
    }

    private async Task WriteInCorrespondingFile(string value, string path)
    {
        if (!String.IsNullOrEmpty(path))
        {
            StringBuilder sb = new StringBuilder();
            if (!File.Exists(path))
            {
                string newLineHeader = string.Join(",", "CreatedDate", "Value");
                sb.Append(newLineHeader + Environment.NewLine);
                sb.Append(value + Environment.NewLine);
                await File.WriteAllTextAsync(path, sb.ToString());

            }
            else
            {
                sb.Append(value + Environment.NewLine);
                await File.AppendAllTextAsync(path, sb.ToString());
            }

        }
    }

    private async Task GeneratePredicitonFiles(List<MeasuredData> data)
    {
        var avgN = data.Select(x => x.N).Average();
        var avgP = data.Select(x => x.P).Average();
        var avgK = data.Select(x => x.K).Average();

        await WriteInCorrespondingFile(avgN.ToString(), _optionsMonitor.CurrentValue.PredictFilePathN);
        await WriteInCorrespondingFile(avgP.ToString(), _optionsMonitor.CurrentValue.PredictFilePathP);
        await WriteInCorrespondingFile(avgK.ToString(), _optionsMonitor.CurrentValue.PredictFilePathK);

    }
}

