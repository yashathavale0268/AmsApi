using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using Microsoft.Extensions.Configuration;

namespace AmsApi.Repository
{
    public class AmsRepository
    {
        //private readonly string _connectionString;


        //public AmsRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("MainCon");
        //}
        //internal async Task giveAll()
        //{
        //     var connectionString = "<your-connection-string>";
        //    var query = "SELECT id, name FROM names";

        //    using (var connection = new SqlConnection(connectionString))
        //    using (var command = new SqlCommand(query, connection))
        //    {
        //        connection.Open();

        //        var common = new List<Commondbo>();

        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                var common = new Commondbo
        //                {
        //                    Id = reader.GetInt32(0),
        //                    Name = reader.GetString(1)
        //                };

        //                common.Add(name);
        //            }
        //        }

        //        return Ok(names);
        //    }
        //    public Commondbo MapToValue(SqlDataReader reader)
        //    {
        //        return new Commondbo()
        //        {
        //            Id = (int)reader,
                  
        //            Name = reader.ToString(),
                   
        //        };

        //    }
        //    using (SqlConnection sql = new(_connectionString))
        //    {
        //        using (SqlCommand cmd = new("sp_GetAll", sql))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            var response = new List<Commondbo>();
        //            await sql.OpenAsync();

        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    var common = new Commondbo
        //                    {
        //                        Id = reader.GetInt32(0),
        //                        Name = reader.GetString(1)
        //                    };

        //                    common.Add(reader);
        //                }
        //            }

        //            return ;
        //        }
        //    }
        }
}
