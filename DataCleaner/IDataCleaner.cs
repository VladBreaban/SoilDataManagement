namespace DataCleaner;

public interface IDataCleaner
{
    Task<string> GetCleanData(string fileToBeCleanedPath);
}

