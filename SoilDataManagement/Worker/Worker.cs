namespace SoilDataManagement.Worker;

public class Worker : IWorker
{
    private readonly ILogger<Worker> _logger;
    private readonly IDataManager _dataManager;
    private readonly IDataCleaner _dataCleaner;
    private readonly IElasticHelper _elasticHelper;
    public Worker(ILogger<Worker> logger, IDataManager dataManager, IDataCleaner dataCleaner, IElasticHelper elasticHelper)
    {
        _logger = logger;
        _dataManager = dataManager;
        _dataCleaner = dataCleaner;
        _elasticHelper = elasticHelper;
    }
    public async Task DoWork(CancellationToken cancelToken)
    {
        await Task.Yield();
        // to do--> make it configurable
        TimeSpan start = new TimeSpan(20, 0, 0); //10 o'clock
        TimeSpan end = new TimeSpan(20,10, 0); //12 o'clock
        while (!cancelToken.IsCancellationRequested)
        {
            var now = DateTime.Now.TimeOfDay;
            if ((now > start) && (now < end))
            {
                //match found --> get data from thinkspeak and send them to elastic server
                var allDataPath = await _dataManager.GetDataBetweenTimeInterval("", "");

                var cleanedData = await _dataCleaner.GetCleanData(allDataPath);

                await _elasticHelper.IndexAsync(new DataManager.Models.MeasuredData(), "soil-data");

            }

        }
    }
}

