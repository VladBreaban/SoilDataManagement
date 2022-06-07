using DataManager.Models;

namespace DataManager.Engine;

public class ElasticHelper : IElasticHelper
{
    private readonly ElasticSearchClient _elasticSearchClient;
    public ElasticHelper(ElasticSearchClient elasticSearchClient)
    {
        _elasticSearchClient = elasticSearchClient;
    }
    public async Task<IndexResponse> IndexAsync<T>(T data, string index, string id = null) where T : class
    {
        try
        {
            var myClient = _elasticSearchClient.GetElasticClient();
            var indexExists = myClient.Indices.Exists(index);
            if (!indexExists.Exists)
            {
                var response = await myClient.Indices.CreateAsync(index,
                   index => index.Map<T>(
                       x => x.AutoMap()
                   ));
            }
            var indexResponse = await myClient.IndexDocumentAsync(data);
            return indexResponse;

        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        return null; // to be deleted asap, onlyfor test

    }
}

