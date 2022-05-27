namespace SoilDataManagement.Controllers;

[ApiController]
[Route("NPKMainController/[action]")]
public class NPKMainController : ControllerBase
{

    private readonly ILogger<NPKMainController> _logger;
    private readonly IDataManager _dataManager;
    private readonly IDataCleaner _dataCleaner;
    private readonly IElasticHelper _elasticHelper;
    public NPKMainController(ILogger<NPKMainController> logger, IDataManager dataManager, IDataCleaner dataCleaner, IElasticHelper elasticHelper)
    {
        _logger = logger;
        _dataManager = dataManager;
        _dataCleaner = dataCleaner;
        _elasticHelper = elasticHelper; 
    }

    [HttpGet]

    public async Task CleanData(string filePath)
    {
        try
        {
            await _dataCleaner.GetCleanData(filePath);

        }
        catch (Exception ex)
        {

        }
    }

    [HttpGet]
    public async Task<string> GetAllDataFromCloud()
    {
        string path = String.Empty;
        try
        {
            path = await _dataManager.GetAllDataFromCloud();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return path;
    }

    [HttpGet]

    public async Task TestController()
    {
        var TEST = await _elasticHelper.IndexAsync(new DataManager.Models.MeasuredData { nitro = 20.ToString(), phosphoros = 50.ToString(), potassium = 20.ToString() }, "soil-data");
        return;
    }
}
