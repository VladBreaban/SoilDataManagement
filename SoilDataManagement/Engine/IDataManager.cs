
namespace SoilDataManagement.Engine
{
    public interface IDataManager
    {
        Task InsertDataToDb();

        Task<string> GetDataFromCloud();
    }
}