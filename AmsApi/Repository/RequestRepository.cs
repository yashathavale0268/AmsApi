﻿using System;
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

        public int totalCount { get; set; }
        public RequestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        //internal async Task<List<RequestModel>> GetAllRequests(int pageNumber, int pageSize)
        //{
        //    using (SqlConnection sql = new(_connectionString))
        //    {
        //        using (SqlCommand cmd = new("sp_GetAllRequests", sql))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //            cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //            var response = new List<RequestModel>();
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

        internal async Task<List<RequestModel>> SearchRequests(int pageNumber, int pageSize,string searchString,int userId ,int reqId, int assetId, int statId,int DateFilter,string StartDate,string EndDate)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchAllRequests_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@UserId ", userId);
                    cmd.Parameters.AddWithValue("@ReqId ", reqId);
                    cmd.Parameters.AddWithValue("@AssetId ", assetId);
                    cmd.Parameters.AddWithValue("@StatId ", statId);
                    cmd.Parameters.AddWithValue("@SearchString", searchString);
                    cmd.Parameters.AddWithValue("@DateFilter", DateFilter);
                    cmd.Parameters.AddWithValue("@StartDate",StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
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
                Requestid = reader.IsDBNull(reader.GetOrdinal("Requestid")) ? 0 : (int)reader["Requestid"],

                Userid = reader.IsDBNull(reader.GetOrdinal("Userid")) ? 0: (int)reader["Userid"],

           //     List<int> Assets = new List<int> {},
           //Assets = reader["Assets"].ToString(),
           UniqueId = reader.IsDBNull(reader.GetOrdinal("UniqueId")) ? null: (string)reader["UniqueId"],
             Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? 0: (int)reader["Type"],
            Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                Justify = reader.IsDBNull(reader.GetOrdinal("Justify")) ? null : (string)reader["Justify"],
                Branch=reader.IsDBNull(reader.GetOrdinal("Branch"))? 0 :(int)reader["Branch"],
                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? 0: (int)reader["Status"],
                active = (bool)reader["active"],
                isworking=(bool)reader["isworking"],
                inuse=(bool)reader["inuse"],
                Asset = reader.IsDBNull(reader.GetOrdinal("Asset")) ? null : (string)reader["Asset"],
                TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? null : (string)reader["TypeName"],
                CurrentStatus = reader.IsDBNull(reader.GetOrdinal("CurrentStatus")) ? null : (string)reader["CurrentStatus"],
                UserName= reader.IsDBNull(reader.GetOrdinal("UserName")) ? null : (string)reader["UserName"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"],
                specialrecord= reader.IsDBNull(reader.GetOrdinal("specialrecord")) ? 0 : (int)reader["specialrecord"],
                BranchName=reader.IsDBNull(reader.GetOrdinal("BranchName"))? null :(string)reader["BranchName"],
            };

        }

        internal DataSet Getassetdropdown(int id)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetDropDownByType", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@typ",id);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }


        //internal async Task Insert(RequestModel request)
        internal async Task Insert(int userid,int asset,string justify)//,int type
        {
            using (SqlConnection sql = new(_connectionString))
            {
                //string.Join(","
                using (SqlCommand cmd = new("sp_RequestCreate", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@Userid", userid);
                    cmd.Parameters.AddWithValue("@Type",asset);
                  //  cmd.Parameters.AddWithValue("@typeid", asset);
                    //cmd.Parameters.AddWithValue("@Created_at", request.Created_at);
                    cmd.Parameters.AddWithValue("@Justify", justify);
                   // cmd.Parameters.AddWithValue("@Status", request.Status);// set a station for evry new request to be set 
                    cmd.Parameters.AddWithValue("@active", 1);

                    await sql.OpenAsync();
                    var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returncode);
                    var returnpart = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
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

        internal async Task StatusChange( bool isworking, bool inuse, int type, int id)//string UniqueId, string SerialNo,
        {
            using (SqlConnection sql = new(_connectionString))
            {
                //string.Join(","
                using (SqlCommand cmd = new("sp_StatusChange", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", comp.Companyid);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@isworking", isworking);
                    cmd.Parameters.AddWithValue("@inuse", inuse);
                    //cmd.Parameters.AddWithValue("@Uniqueid", UniqueId);
                    //cmd.Parameters.AddWithValue("@SerialNo", SerialNo);


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

        internal async Task UniqueIdAdd(string UniqueId, string SerialNo, int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                //string.Join(","
                using (SqlCommand cmd = new("sp_UniqueidAlloted", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", comp.Companyid);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@Uniqueid", UniqueId);
                    cmd.Parameters.AddWithValue("@SerialNo", SerialNo);


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
        internal async Task RequestAccepted(bool isAccepted, int type, int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                //string.Join(","
                using (SqlCommand cmd = new("sp_AcceptRequests", sql)) //sp_AcceptRequests
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", comp.Companyid);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@isAccepted", isAccepted);
                   

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
        internal async Task SendtoScrap(bool isScrap, int type, string Uid,int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                //string.Join(","
                using (SqlCommand cmd = new("sp_SendToScrap", sql))//sp_StatusChange
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@Uid", Uid);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@isScrap", isScrap);


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

        internal async Task SentForFix(bool SentForFix, int type,int Vendorid, string Uid , string Description, int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                //string.Join(","
                using (SqlCommand cmd = new("sp_SentForFix", sql))//sp_StatusChange
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@Uid", Uid);
                    cmd.Parameters.AddWithValue("@Desc", Description);
                    cmd.Parameters.AddWithValue("@Vendorid", Vendorid);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@SentForFix", SentForFix);


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
                using (SqlCommand cmd = new("sp_SearchAllRequests_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ReqId", id));
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

        internal async Task UpdateRequest(int userid,int asset,string justify,int id)//int type
        {
           
                using (SqlConnection sql = new(_connectionString))
                {
                    using (SqlCommand cmd = new("sp_RequestCreate", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@Userid", userid);
                        cmd.Parameters.AddWithValue("@Type", asset);
                        //cmd.Parameters.AddWithValue("@typeid", type);
                    //  cmd.Parameters.AddWithValue("@Created_at", request.Created_at);
                    cmd.Parameters.AddWithValue("@Justify", justify);
                       // cmd.Parameters.AddWithValue("@Status", request.Status);
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
