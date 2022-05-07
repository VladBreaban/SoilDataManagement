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
}
