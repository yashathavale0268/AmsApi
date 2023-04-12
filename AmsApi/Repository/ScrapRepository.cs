using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AmsApi.Repository
{
    public class ScrapRepository
    {
        private readonly string _connectionString;
        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }
        public ScrapRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        //internal async Task<List<ScrapModel>> GetAllScraps()
        //{
        //    using (SqlConnection sql = new(_connectionString))
        //    {
        //        using (SqlCommand cmd = new("sp_GetAllScrap", sql))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            var response = new List<ScrapModel>();
        //            await sql.OpenAsync();

        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    response.Add(MapToValue(reader));
        //                }
        //            }

        //            return response;
        //        }
        //    }
        //}

        public DataSet GetAllTables()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAll", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }

        //internal async Task<List<ScrapModel>> GetAllScrap(int pageNumber, int pageSize)
        //{
        //    using SqlConnection sql = new(_connectionString);
        //    using SqlCommand cmd = new("sp_GetAllScrap", sql);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //    cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //    var response = new List<ScrapModel>();
        //    await sql.OpenAsync();

        //    using (var reader = await cmd.ExecuteReaderAsync())
        //    {
        //        while (await reader.ReadAsync())
        //        {
        //            response.Add(MapToValue(reader));
        //        }
        //    }

        //    return response;
        //}
            internal async Task<List<ScrapModel>> SearchScrap(int pageNumber, int pageSize, string searchTerm, int searchId, int assetid, int brcid, int vedid, int userid, int DateFilter)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllScrap_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                    cmd.Parameters.AddWithValue("@Searchid", searchId);
                    cmd.Parameters.AddWithValue("@Assetid", assetid);
                    cmd.Parameters.AddWithValue("@Brcid", brcid);
                    cmd.Parameters.AddWithValue("@Vedid", vedid);
                    cmd.Parameters.AddWithValue("@Userid", userid);
                    cmd.Parameters.AddWithValue("@DateFilter", DateFilter);
                    var response = new List<ScrapModel>();
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

        private ScrapModel MapToValue(SqlDataReader reader)
        {
            return new ScrapModel()
            {
                Scrapid = (int)reader["Scrapid"],
                Asset = reader.IsDBNull(reader.GetOrdinal("SerialNo")) ? 0 : (int)reader["Asset"],
              
                Branch = reader.IsDBNull(reader.GetOrdinal("SerialNo")) ? 0: (int)reader["Branch"],
               
                Last_user = reader.IsDBNull(reader.GetOrdinal("SerialNo")) ? 0 : (int)reader["Last_user"],
               
                Vendor = reader.IsDBNull(reader.GetOrdinal("SerialNo")) ? 0 : (int)reader["Vendor"],
                
                Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,

                active = (bool)reader["active"],
                AssetName = reader.IsDBNull(reader.GetOrdinal("AssetName")) ? "N/A" : (string)reader["AssetName"],
                BranchName = reader.IsDBNull(reader.GetOrdinal("BranchName")) ? "N/A" : (string)reader["BranchName"],
                VendorName = reader.IsDBNull(reader.GetOrdinal("VendorName")) ? "N/A" : (string)reader["VendorName"],
                LastUser = reader.IsDBNull(reader.GetOrdinal("LastUser")) ? "N/A" : (string)reader["LastUser"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"]
            };
        }

        internal async Task<List<ScrapModel>> GetScrapId(ScrapModel scp)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllScrap_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Searchid", scp.Scrapid));
                //    ScrapModel response = null;
                    var response = new List<ScrapModel>();
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
        internal async Task<List<ScrapModel>> GetId(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllScrap_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Searchid",id));
                    //    ScrapModel response = null;
                    var response = new List<ScrapModel>();
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

        internal async Task Insert(ScrapModel scrap)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_ScrapCreate", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", comp.Companyid);
                    cmd.Parameters.AddWithValue("@Asset", scrap.Asset);
                    cmd.Parameters.AddWithValue("@Branch", scrap.Branch);
                    cmd.Parameters.AddWithValue("@Last_user", scrap.Last_user);
                    cmd.Parameters.AddWithValue("@Vendor", scrap.Vendor);
                    cmd.Parameters.AddWithValue("@Created_at", scrap.Created_at);
                    cmd.Parameters.AddWithValue("@active", 1);

                    await sql.OpenAsync();
                    var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returncode);
                    var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returnpart);
                    await cmd.ExecuteNonQueryAsync();
                    bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
                    bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
                    Itexists = itExists;
                    IsSuccess = isSuccess;
                    return;
                }
            }
        }

        internal async Task UpdateScrap(ScrapModel scrap)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_ScrapCreate", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id",scrap.Scrapid);
                    cmd.Parameters.AddWithValue("@Asset", scrap.Asset);
                    cmd.Parameters.AddWithValue("@Branch", scrap.Branch);
                    cmd.Parameters.AddWithValue("@Last_user", scrap.Last_user);
                    cmd.Parameters.AddWithValue("@Vendor", scrap.Vendor);
                    cmd.Parameters.AddWithValue("@Created_at", scrap.Created_at);
                    cmd.Parameters.AddWithValue("@active", 1);

                    await sql.OpenAsync();
                    var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    //  cmd.Parameters.Add(returncode);
                    var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returnpart);
                    await cmd.ExecuteNonQueryAsync();
                    //     bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
                    bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
                    //  Itexists = itExists;
                    IsSuccess = isSuccess;
                    return;
                }
            }
        }

        internal async Task DeleteById(int id)
        {

            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_DeleteScrap", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }
    }
}
