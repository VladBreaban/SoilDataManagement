
using DataManager.Models;

namespace DataManager;

    public interface IDataBaseService
    {
    Task InserToDataBase(List<MeasuredData> data);
    }

