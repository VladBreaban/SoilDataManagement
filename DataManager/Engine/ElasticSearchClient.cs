
namespace DataManager.Engine;

public class ElasticSearchClient
{
    private string _uri;

    public ElasticSearchClient(string uri)
    {
        _uri = uri;
    }

    public ElasticClient GetElasticClient()
    {
        var nodes = new Uri[]
            {
                new Uri(_uri),
            };
        var connectionPool = new StaticConnectionPool(nodes);
        var connectionSettings = new ConnectionSettings(connectionPool)
            //do not hardcode, move to appsettings replacebale during yml, WIP
                               .BasicAuthentication("elastic", "1adminElastic").DisableDirectStreaming();
        var elasticClient = new ElasticClient(connectionSettings.DefaultIndex("soil-data"));
        return elasticClient;
    }
}

