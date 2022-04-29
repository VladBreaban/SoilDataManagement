using Microsoft.AspNetCore.Mvc;
using SoilDataManagement.Models;

namespace SoilDataManagement.Controllers
{
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
        public bool InsertToDb(string val1,string val2)
        {
            var test = _dataManager.GetDataFromCloud();
            _logger.LogInformation("Hello world");
            return true;    
        }
    }
}