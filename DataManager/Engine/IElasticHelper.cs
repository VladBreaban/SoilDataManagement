using DataManager.Models;

namespace DataManager.Engine;

    public interface IElasticHelper
    {
    Task<IndexResponse> IndexAsync(MeasuredData data, string index, string id = null);
   }

