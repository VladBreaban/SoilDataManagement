using DataManager.MachineLearning;

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
    public async Task<string> GetDataToPredict(string desiredField)
    {
        string path = String.Empty;
        try
        {
            var allDatapath = await _dataManager.GetAllDataFromCloud();

            path = await _dataCleaner.GenerateCleanDataFileForACertainField(allDatapath, desiredField);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return path;
    }
    [HttpGet]
    public async Task<string> PredictDataOverXYear()
    {
        string path = String.Empty;
        try
        {
            _mlPredictor.TrainAndPredict(@"C:\Users\Vlad\Desktop\net6\20220507cleanData.csv", @"C:\Users\Vlad\Desktop\net6\MLModel.zip");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return path;
    }

}
