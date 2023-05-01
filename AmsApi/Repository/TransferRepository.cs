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
    public class TransferRepository
    {

        private readonly string _connectionString;

        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }

        public TransferRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
       public async Task<List<TransferModel>> GetId(int id)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_SearchAllTransfers_Paginated", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id",id));

            var response = new List<TransferModel>();
            await sql.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    response.Add(MapToValue(reader));
                }
            }

            return response;
        }

        public TransferModel MapToValue(SqlDataReader reader)
        {
            return new TransferModel()
            {
                TrId = (int)reader["TrId"],
                
                Branch = reader.IsDBNull(reader.GetOrdinal("Branch")) ? 0 : (int)reader["Branch"],
               
                Qty = reader.IsDBNull(reader.GetOrdinal("Qty")) ? 0 : (int)reader["Qty"],
                Description = reader.IsDBNull(reader.GetOrdinal("Specification")) ? "" : (string)reader["Specification"],
                
                TrfBranchName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? "" : (string)reader["StatusName"],
                //Allocated_to=(int)reader["Allocated_to"],
               
                Transferd_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                totalrecord = (int)reader["totalrecord"]


                // Lastuser = reader["Lastuser"].ToString(),
            };
        }

        public async Task<List<TransferModel>> SearchTransfers(int pageNumber, int pageSize)
        {  
                using SqlConnection sql = new(_connectionString);
                using SqlCommand cmd = new("sp_SearchAllTransfers_Paginated", sql);
                /*sp_SearchAllAssets_Paginated*/
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                //cmd.Parameters.AddWithValue("@rtype", rtype);
                //cmd.Parameters.AddWithValue("@btype", btype);
                var response = new List<TransferModel>();
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        response.Add(MapToValue(reader));
                    }
                }

                return response;
            
        }
    }
}
