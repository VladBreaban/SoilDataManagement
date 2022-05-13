namespace DataManager.Engine;

public class ElasticClient : IElasticClient
{
    public Task<RestResponse> IndexAsync<T>(T data, string index, string id = null)
    {
        throw new NotImplementedException();
    }
}

