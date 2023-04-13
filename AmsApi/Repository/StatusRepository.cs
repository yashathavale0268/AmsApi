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
    public class StatusRepository
    {
        private readonly string _connectionString;
        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }
        public StatusRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        //internal async Task<List<StatusModel>> GetAllStatus()
        //{
        //    using (SqlConnection sql = new(_connectionString))
        //    {
        //        using (SqlCommand cmd = new("sp_GetAllStatus", sql))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            var response = new List<StatusModel>();
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

        //internal async Task<List<StatusModel>> GetAllStatus(int pageNumber, int pageSize)
        //{
        //    using SqlConnection sql = new(_connectionString);
        //    using SqlCommand cmd = new("sp_GetAllStatus", sql);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //    cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //    var response = new List<StatusModel>();
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

        public StatusModel MapToValue(SqlDataReader reader)
        {
            return new StatusModel()
            {
               Statusid = (int)reader["Statusid"],
               Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? 0 : (int)reader["Status"],
                Userid = reader.IsDBNull(reader.GetOrdinal("Userid")) ? 0 : (int)reader["Userid"],
                Assetid = reader.IsDBNull(reader.GetOrdinal("Assetid")) ? 0 : (int)reader["Assetid"],
                Requestid = reader.IsDBNull(reader.GetOrdinal("Requestid")) ? 0 : (int)reader["Requestid"],
                Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                active = (bool)reader["active"],
                StatusNow = reader.IsDBNull(reader.GetOrdinal("StatusNow")) ? "N/A" : (string)reader["StatusNow"],
                Asset = reader.IsDBNull(reader.GetOrdinal("Asset")) ? "N/A" : (string)reader["Asset"],
                Request = reader.IsDBNull(reader.GetOrdinal("Request")) ? "N/A" : (string)reader["Request"],
                UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? "N/A" : (string)reader["UserName"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0: (int)reader["totalrecord"]
            };
        }

        internal async Task<List<StatusModel>> SearchStatus(int pageNumber,int pageSize,string searchTerm,int Userid,int Assetid,int Requestid,int Statid,int DateFilter,string StartDate,string EndDate )
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchAllStatus_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                    cmd.Parameters.AddWithValue("@Userid", Userid);
                    cmd.Parameters.AddWithValue("@Assetid", Assetid);
                    cmd.Parameters.AddWithValue("@Requestid", Requestid);
                    cmd.Parameters.AddWithValue("@Statid", Statid);
                    cmd.Parameters.AddWithValue("@DateFilter", DateFilter);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    var response = new List<StatusModel>();
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

        internal async Task<List<StatusModel>> GetStatusById(StatusModel stat)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllStatus_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Statid", stat.Statusid));
                    //StatusModel response = null;
                    var response = new List<StatusModel>();
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
        internal async Task<List<StatusModel>> GetId(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllStatus_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Statid", id));
                    //StatusModel response = null;
                    var response = new List<StatusModel>();
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

        internal async Task Insert(StatusModel stat)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_CompanyCreate", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", stat.Statusid);
                    cmd.Parameters.AddWithValue("@Userid", stat.Userid);
                    cmd.Parameters.AddWithValue("@Assetid", stat.Assetid);
                    cmd.Parameters.AddWithValue("@Requestid", stat.Requestid);
                    cmd.Parameters.AddWithValue("@Creat_at", stat.Created_at);
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

        internal async Task DeleteById(int id)
        {

            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_DeleteStatus", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        internal async Task UpdateStatus(StatusModel stat)
        {
            try
            { 
                    using (SqlConnection sql = new(_connectionString))
                    {
                         using (SqlCommand cmd = new("sp_StatusCreate", sql))
                         {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@id", stat.Statusid);
                                cmd.Parameters.AddWithValue("@Userid", stat.Userid);
                                cmd.Parameters.AddWithValue("@Assetid", stat.Assetid);
                                cmd.Parameters.AddWithValue("@Requestid", stat.Requestid);
                                cmd.Parameters.AddWithValue("@Creat_at", stat.Created_at);
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
            catch (Exception)
            {

            }
}
    }
}
