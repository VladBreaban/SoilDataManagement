using RestSharp;

namespace SoilDataManagement.Engine
{
    public class DataManager : IDataManager
    {
        private readonly ILogger<DataManager> _logger;
        private readonly IOptionsMonitor<ThingSpeakOptionsMonitor> _thingSpeakOptionsMonitor;
        public DataManager(ILogger<DataManager> logger, IOptionsMonitor<ThingSpeakOptionsMonitor> thingSpeakOptionsMonitor)
        {
            _logger = logger;
            _thingSpeakOptionsMonitor = thingSpeakOptionsMonitor;
        }

        public async Task<string> GetDataFromCloud()
        {
            var result = "";
            var endpoint = String.Format(_thingSpeakOptionsMonitor.CurrentValue.UrlFormat, _thingSpeakOptionsMonitor.CurrentValue.ChannelId, _thingSpeakOptionsMonitor.CurrentValue.FileNname);
            try
            {
                var restRequest = new RestRequest(endpoint,Method.Get)
                                    //  .AddParameter("all", false, ParameterType.QueryString)
                                      .AddHeader("content-type", "application/json");
                var restClient = new RestClient(_thingSpeakOptionsMonitor.CurrentValue.BaseUrl);
                var response =await restClient.ExecuteAsync(restRequest);
                string content = response.Content;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occured while trying to get date from cloud, used endpoint {endpoint}. Time:{DateTime.Now}");
            }
            return result;
        }

        public async Task InsertDataToDb()
        {

        }
    }
}
