using DataManager.Models;

namespace DataManager.Engine;

    public interface IElasticHelper
    {
    Task<IndexResponse> IndexAsync<T>(T data, string index, string id = null) where T : class;
   }

