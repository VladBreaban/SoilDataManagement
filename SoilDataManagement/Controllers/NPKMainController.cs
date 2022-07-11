using DataManager;
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
    private readonly IDataBaseService _dbService;
    public NPKMainController(ILogger<NPKMainController> logger, IDataManager dataManager, IDataCleaner dataCleaner, IDataBaseService dbService)
    {
        _logger = logger;
        _dataManager = dataManager;
        _dataCleaner = dataCleaner;
        _dbService = dbService;
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
        string path = @"C:\Users\Vlad\Downloads\feeds.csv";
        try
        {
           // path = await _dataManager.GetAllDataFromCloud();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        var result = await _dataCleaner.GetCleanDataAverageValues(path);
        await _dbService.InserToDataBase(result);
        return result;

    }

    [HttpGet]

    public async Task<List<MeasuredData>> GetDataBetweenTimeInterval(DateTime startDate, DateTime endDate)
    {
        var allDataPath = await _dataManager.GetDataBetweenTimeInterval(startDate.ToString(), endDate.ToString());
     
        return await _dataCleaner.GetCleanData(allDataPath);
        
    }

}
