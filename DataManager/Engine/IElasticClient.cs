namespace DataManager.Engine;

public interface IElasticClient
{
    Task<RestResponse> IndexAsync<T>(T data, string index, string id = null);
}

