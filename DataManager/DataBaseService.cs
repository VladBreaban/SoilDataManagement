
using Dapper;
using DataManager.Models;
using System.Data.SqlClient;

namespace DataManager;

public class DataBaseService : IDataBaseService
{
    private readonly string cs = "Data Source=DESKTOP-OIPLHJT;Initial Catalog=SoilMonitor;Integrated Security=True;";
    public async Task InserToDataBase(List<MeasuredData> data)
    {
        using (var con = new SqlConnection(cs))
        {
            con.Open();

            foreach (var item in data)
            {
                string processQuery = "INSERT INTO MeasuredData VALUES (@N,@P,@K,@Date)";
               await con.ExecuteAsync(processQuery, new {N = item.N, P = item.P, K=item.K, Date=item.MeasuredDate });
            }
        }

    }

    public async Task<List<MeasuredData>> GetAllDataFromDatabase()
    {
        List<MeasuredData> measuredData = new();
        using (var con = new SqlConnection(cs))
        {
            con.Open();
            string query = "SELECT [N], [P], [K], [MeasuredDate] FROM [SoilMonitor].[dbo].[MeasuredData]";
            SqlCommand command = new SqlCommand(query, con);
            using(var reader = command.ExecuteReader())
            {
                var parser = reader.GetRowParser<MeasuredData>(typeof(MeasuredData));
                while (reader.Read())
                {
                    measuredData.Add(parser(reader));
                }
            }
        }
        return measuredData;
    }

    public async Task<List<MeasuredData>> GetDataBetweenTimeIntervalFromDatabase(DateTime startDate, DateTime endDate)
    {
        List<MeasuredData> measuredData = new();
        using (var con = new SqlConnection(cs))
        {
            con.Open();
            string query = "SELECT [N], [P], [K], [MeasuredDate] FROM [SoilMonitor].[dbo].[MeasuredData]";
            SqlCommand command = new SqlCommand(query, con);
            using (var reader = await command.ExecuteReaderAsync())
            {
                var parser = reader.GetRowParser<MeasuredData>(typeof(MeasuredData));
                while (reader.Read())
                {
                    measuredData.Add(parser(reader));
                }
            }
        }
        measuredData = measuredData.Where(i => DateTime.Compare(startDate, i.MeasuredDate) <= 0 && DateTime.Compare(endDate, i.MeasuredDate) >= 0).ToList();
        return measuredData;
    }
}

