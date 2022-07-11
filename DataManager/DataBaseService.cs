
using Dapper;
using DataManager.Models;
using System.Data.SqlClient;

namespace DataManager;

public class DataBaseService : IDataBaseService
{

    public async Task InserToDataBase(List<MeasuredData> data)
    {
        var cs = @"Server=DESKTOP-OIPLHJT;Database=SoilMonitor;Trusted_Connection=True;";

        using (var con = new SqlConnection(cs))
        {
            con.Open();

            foreach (var item in data)
            {
                string processQuery = "INSERT INTO MeasuredData VALUES (@N,@P,@K,@Date)";
               await con.ExecuteAsync(processQuery, new {N = item.N, P = item.P, K=item.K, Date=item.CreatedDate });
            }
        }

    }
}

