namespace DataManager.Engine;

public interface IDataManager
{

    Task<string> GetAllDataFromCloud();

    Task<string> GetDataBetweenTimeInterval(string startDate, string endDate);
}
