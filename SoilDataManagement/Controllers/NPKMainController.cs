using Microsoft.AspNetCore.Mvc;

namespace SoilDataManagement.Controllers
{
    [ApiController]
    [Route("NPKMainController/[action]")]
    public class NPKMainController : ControllerBase
    {

        private readonly ILogger<NPKMainController> _logger;
        private readonly DataManager _dataManager;

        public NPKMainController(ILogger<NPKMainController> logger, DataManager dataManager)
        {
            _logger = logger;
            _dataManager = dataManager; 
        }

        [HttpPost(Name = "InsertToDb")]
        public bool InsertToDb(string nitro,string phosphoros, string potassium)
        {
            _logger.LogInformation("Hello world");
            return true;    
        }
    }
}