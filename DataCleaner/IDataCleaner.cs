using DataManager.Models;

namespace DataCleaner;

public interface IDataCleaner
{
    Task<List<MeasuredData>> GetCleanData(string fileToBeCleanedPath);
}

