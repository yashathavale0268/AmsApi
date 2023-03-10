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
    public class RequestRepository
    {
        private readonly string _connectionString;
        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }
        public RequestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        internal async Task<List<RequestModel>> GetAllRequests(int pageNumber, int pageSize)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_GetAllRequests", sql))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    var response = new List<RequestModel>();
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

        internal async Task<List<RequestModel>> SearchRequests(int pageNumber, int pageSize, int searchTerm,string searchString, int reqId, int assetId, int statId)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchAllRequests_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@ReqId ", reqId);
                    cmd.Parameters.AddWithValue("@AssetId ", assetId);
                    cmd.Parameters.AddWithValue("@StatId ", statId);
                    cmd.Parameters.AddWithValue("@SearchString", searchString);
                    var response = new List<RequestModel>();
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

        private RequestModel MapToValue(SqlDataReader reader)
        {
            return new RequestModel()
            {
                Requestid = (int)reader["Requestid"],

                Userid = (int)reader["Userid"],

           //     List<int> Assets = new List<int> {},
           //Assets = reader["Assets"].ToString(),
           Assetid = (int)reader["Assetid"],
            Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                Justify = reader["Justify"].ToString(),
                Status =(int)reader["Status"],
                active = (bool)reader["active"],
                isworking=(bool)reader["isworking"],
                inuse=(bool)reader["inuse"],
                Asset = (string)reader["Asset"],
                CurrentStatus = reader["CurrentStatus"].ToString(),
                UserName=(string)reader["UserName"],
            };

        }

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

        internal async Task Insert(RequestModel request)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                //string.Join(","
                using (SqlCommand cmd = new("sp_RequestCreate", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", comp.Companyid);
                    cmd.Parameters.AddWithValue("@Userid", request.Userid);
                    cmd.Parameters.AddWithValue("@Assetid",request.Assetid);
                    cmd.Parameters.AddWithValue("@Created_at", request.Created_at);
                    cmd.Parameters.AddWithValue("@Justify", request.Justify);
                    cmd.Parameters.AddWithValue("@Status", request.Status);// set a station for evry new request to be set 
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

        internal async Task StatusChange(bool isworking, bool inuse, int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                //string.Join(","
                using (SqlCommand cmd = new("sp_StatusChange", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", comp.Companyid);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@isworking", isworking);
                    cmd.Parameters.AddWithValue("@inuse", inuse);

                    var returnpart = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returnpart);
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await sql.CloseAsync();
                    // bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
                    bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
                    //  Itexists = itExists;
                    IsSuccess = isSuccess;
                }
                return;
            }
        }
        internal async Task<List<RequestModel>> GetRequestId(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_GetAllRequests", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                   // RequestModel response = null;
                    var response = new List<RequestModel>();
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

        internal async Task UpdateRequest(RequestModel request, int id)
        {
           
                using (SqlConnection sql = new(_connectionString))
                {
                    using (SqlCommand cmd = new("sp_RequestCreate", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@Userid", request.Userid);
                        cmd.Parameters.AddWithValue("@Assetid", request.Assetid);
                        cmd.Parameters.AddWithValue("@Created_at", request.Created_at);
                        cmd.Parameters.AddWithValue("@Justify", request.Justify);
                        cmd.Parameters.AddWithValue("@Status", request.Status);
                        cmd.Parameters.AddWithValue("@active", 1);
                        await sql.OpenAsync();
                       // var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                        //  cmd.Parameters.Add(returncode);
                        var returnpart = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(returnpart);
                        await cmd.ExecuteNonQueryAsync();
                           // bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
                        bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
                       //  Itexists = itExists;
                        IsSuccess = isSuccess;
                        
                    }
                
            }

            return;
        }

        internal async Task DeleteById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_DeleteRequests", sql))
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
