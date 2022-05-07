namespace DataManager.Engine;

public interface IDataManager
{
    Task InsertDataToDb();

    Task<string> GetAllDataFromCloud();
}
