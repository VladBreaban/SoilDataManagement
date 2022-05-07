namespace DataGetter.OptionMonitor;

public interface IDataManager
{
    Task InsertDataToDb();

    Task<string> GetAllDataFromCloud();
}
