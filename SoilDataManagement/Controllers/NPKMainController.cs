using DataManager.MachineLearning;
using DataManager.Models;

namespace SoilDataManagement.Controllers;

[ApiController]
[Route("NPKMainController/[action]")]
public class NPKMainController : ControllerBase
{

    private readonly ILogger<NPKMainController> _logger;
    private readonly IDataManager _dataManager;
    private readonly IDataCleaner _dataCleaner;
    private readonly IElasticHelper _elasticHelper;

    private readonly IMLPredictor _mlPredictor;
    public NPKMainController(ILogger<NPKMainController> logger, IDataManager dataManager, IDataCleaner dataCleaner, IElasticHelper elasticHelper, IMLPredictor mlPredictor)
    {
        _logger = logger;
        _dataManager = dataManager;
        _dataCleaner = dataCleaner;
        _elasticHelper = elasticHelper; 
        _mlPredictor = mlPredictor;
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
            _logger.LogError(ex, ex.Message);

        }
    }

    [HttpGet]
    public async Task<List<MeasuredData>> GetAllDataFromCloud()
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
        return await _dataCleaner.GetCleanData(path);

    }

    [HttpGet]

    public async Task<List<MeasuredData>> GetDataBetweenTimeInterval(DateTime startDate, DateTime endDate)
    {
        var allDataPath = await _dataManager.GetDataBetweenTimeInterval(startDate.ToString(), endDate.ToString());
     
        return await _dataCleaner.GetCleanData(allDataPath);
        
    }

}
