namespace SoilDataManagement.Engine
{
    public class DataManager : IDataManager
    {
        private readonly ILogger<DataManager> _logger;

        public DataManager(ILogger<DataManager> logger)
        {
            _logger = logger;
        }

        public async Task InsertDataToDb()
        {

        }
    }
}
