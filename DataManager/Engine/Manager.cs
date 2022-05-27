﻿namespace DataManager.Engine;

public class Manager : IDataManager
{
    private readonly ILogger<Manager> _logger;
    private readonly IOptionsMonitor<ThingSpeakOptionsMonitor> _thingSpeakOptionsMonitor;
    public Manager(ILogger<Manager> logger, IOptionsMonitor<ThingSpeakOptionsMonitor> thingSpeakOptionsMonitor)
    {
        _logger = logger;
        _thingSpeakOptionsMonitor = thingSpeakOptionsMonitor;
    }

    public async Task<string> GetAllDataFromCloud()
    {
        var result = "";
        var endpoint = String.Format(_thingSpeakOptionsMonitor.CurrentValue.UrlFormat, _thingSpeakOptionsMonitor.CurrentValue.ChannelId, _thingSpeakOptionsMonitor.CurrentValue.FileNname);
        try
        {
            _logger.LogInformation($"Getting data from {endpoint}");
            var restRequest = new RestRequest(endpoint, Method.Get)
                                  //  .AddParameter("all", false, ParameterType.QueryString)
                                  .AddHeader("content-type", "application/json");
            ;
            var restClient = new RestClient(_thingSpeakOptionsMonitor.CurrentValue.BaseUrl);
            var responseBytes = await restClient.DownloadDataAsync(restRequest);
            var path = Path.Combine(_thingSpeakOptionsMonitor.CurrentValue.SaveFilePath, DateTime.Now.ToString("yyyyMMdd") + "values.csv");
            await File.WriteAllBytesAsync(path, responseBytes);
            if (File.Exists(path))
            {
                return path;
            }
            else
            {
                var message = $"Something went wrong, the file was not properly saved. RestUrl {endpoint}";
                _logger.LogError(message);
                throw new Exception(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occured while trying to get date from cloud, used endpoint {endpoint}. Time:{DateTime.Now}");
        }
        return result;
    }

    public async Task InsertDataToDb()
    {

    }
}
