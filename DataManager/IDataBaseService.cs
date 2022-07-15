
using DataManager.Models;

namespace DataManager;

public interface IDataBaseService
{
    Task InserToDataBase(List<MeasuredData> data);
    Task<List<MeasuredData>> GetAllDataFromDatabase();
    Task<List<MeasuredData>> GetDataBetweenTimeIntervalFromDatabase(DateTime startDate, DateTime endDate);
}

