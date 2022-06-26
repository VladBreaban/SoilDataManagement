namespace DataCleaner;

public interface IDataCleaner
{
    Task<List<MeasuredData>> GetCleanData(string fileToBeCleanedPath);

    Task<string> GenerateCleanDataFileForACertainField(string path, string field);
}

