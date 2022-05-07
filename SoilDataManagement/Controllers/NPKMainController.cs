namespace SoilDataManagement.Controllers;

[ApiController]
[Route("NPKMainController/[action]")]
public class NPKMainController : ControllerBase
{

    private readonly ILogger<NPKMainController> _logger;
    private readonly IDataManager _dataManager;

    public NPKMainController(ILogger<NPKMainController> logger, IDataManager dataManager)
    {
        _logger = logger;
        _dataManager = dataManager;
    }

    [HttpGet]
    public bool GetAllDataFromCloud()
    {
        var result = true;
        try
        {
            var test = _dataManager.GetAllDataFromCloud();
        }
        catch (Exception ex)
        {
            result = false;
            _logger.LogError(ex, ex.Message);
        }

        return result;
    }
}
