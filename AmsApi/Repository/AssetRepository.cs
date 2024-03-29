﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AmsApi.Interfaces;
namespace AmsApi.Repository
{
    public class AssetRepository//:IAssetRepository
    {
        private readonly string _connectionString;

        public bool Itexists { get;  set; }
        public bool IsSuccess { get; set; }

        public AssetRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }

        //public async Task<List<AssetModel>> GetAll()
        //{
        //    using (SqlConnection sql = new SqlConnection(_connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("sp_GetAllAssets", sql))
        //        {
        //            cmd.CommandType =CommandType.StoredProcedure;
        //            var response = new List<AssetModel>();
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

        public DataSet GetAllUsers()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllUsers", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter useradapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                useradapter.Fill(dataSet);

                return dataSet;
            }
        }
        public AssetModel MapToValue(SqlDataReader reader)
        {
            return new AssetModel() {
                Assetid = (int)reader["Assetid"],
                SerialNo = reader.IsDBNull(reader.GetOrdinal("SerialNo")) ? null : (string)reader["SerialNo"],
                Branch = reader.IsDBNull(reader.GetOrdinal("Branch")) ? 0 : (int)reader["Branch"],
                Branches = reader.IsDBNull(reader.GetOrdinal("Branches")) ? null : (string)reader["Branches"],
                Brand = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : (string)reader["Brand"],
                Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? 0 : (int)reader["Type"],
                TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? null : (string)reader["TypeName"],
                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : (string)reader["Model"],
                Processor_Type = reader.IsDBNull(reader.GetOrdinal("Processor_Type")) ? null : (string)reader["Processor_Type"],

                Monitor_Type = reader.IsDBNull(reader.GetOrdinal("Monitor_Type")) ? null : (string)reader["Monitor_Type"],
                Range_Type = reader.IsDBNull(reader.GetOrdinal("Range_Type")) ? null : (string)reader["Range_Type"],
                Battery_Type = reader.IsDBNull(reader.GetOrdinal("Battery_Type")) ? null : (string)reader["Battery_Type"],
                Battery_Ampere = reader.IsDBNull(reader.GetOrdinal("Battery_Ampere")) ? null : (string)reader["Battery_Ampere"],
                Battery_Capacity = reader.IsDBNull(reader.GetOrdinal("Battery_Capacity")) ? null : (string)reader["Battery_Capacity"],
                GraphicsCard = reader.IsDBNull(reader.GetOrdinal("GraphicsCard")) ? null : (string)reader["GraphicsCard"],
                Optical_Drive = reader.IsDBNull(reader.GetOrdinal("Optical_Drive")) ? null : (string)reader["Optical_Drive"],
                HDD = reader.IsDBNull(reader.GetOrdinal("HDD")) ? null : (string)reader["HDD"],
                RAM = reader.IsDBNull(reader.GetOrdinal("RAM")) ? null : (string)reader["RAM"],
                Inches = reader.IsDBNull(reader.GetOrdinal("Inches")) ? null : (string)reader["Inches"],
                Port_Switch = reader.IsDBNull(reader.GetOrdinal("Port_Switch")) ? null : (string)reader["Port_Switch"],
                //Nos = reader.IsDBNull(reader.GetOrdinal("Nos")) ? 0 : (int)reader["Nos"],
                LastAllocated_To = reader.IsDBNull(reader.GetOrdinal("LastAllocated_To")) ? 0 : (int)reader["LastAllocated_To"],
                Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : (string)reader["Specification"],
                Vendorid = reader.IsDBNull(reader.GetOrdinal("Vendorid")) ? 0 : (int)reader["Vendorid"],
                Vendors = reader.IsDBNull(reader.GetOrdinal("Vendors")) ? null : (string)reader["Vendors"],
                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? 3 : (int)reader["Status"],
                StatusName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? null : (string)reader["StatusName"],
                Allocated_At=(reader["Allocated_At"] == DBNull.Value) ? null : Convert.ToDateTime(reader["Allocated_At"]).ToString("yyyy-MM-dd"),
                Sent_On_Date = (reader["Sent_On_Date"] == DBNull.Value) ? null : Convert.ToDateTime(reader["Sent_On_Date"]).ToString("yyyy-MM-dd"),
                Invoice_No = reader.IsDBNull(reader.GetOrdinal("Invoice_No")) ? null : (string)reader["Invoice_No"],
                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? 0 : (int)reader["Location"],
                LocationName = reader.IsDBNull(reader.GetOrdinal("LocationName")) ? null: (string)reader["LocationName"],
                SentToVendor = reader.IsDBNull(reader.GetOrdinal("SentToVendor")) ? null : (string)reader["SentToVendor"],
                Uid = reader.IsDBNull(reader.GetOrdinal("Uid")) ? null : (string)reader["Uid"],
                Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : (string)reader["Remarks"],
                Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                InvoiceDate = (reader["InvoiceDate"] == DBNull.Value) ? null : Convert.ToDateTime(reader["InvoiceDate"]).ToString("yyyy-MM-dd"),
                LastAllocated= reader.IsDBNull(reader.GetOrdinal("LastAllocated")) ? null : (string)reader["LastAllocated"],
                Warranty_Till = (reader["Warranty_Till"] == DBNull.Value) ? null: Convert.ToDateTime(reader["Warranty_Till"]).ToString("yyyy-MM-dd"),
                IsTransfered = (bool)reader["IsTransfered"],
                totalrecord = (int)reader["totalrecord"]
                
               
               // Lastuser = reader["Lastuser"].ToString(),
            };
        }

        public async Task<List<AssetModel>> SearchAssets(int pageNumber, int pageSize, string searchTerm, int Brcid ,int Typeid,int Vendid,int Statid,int DateFilter,string StartDate ,string EndDate)//,string ptype,string mtype,string rtype , string btype )
        {
      
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_SearchAllAssets_Paginated", sql);
            /*sp_SearchAllAssets_Paginated*/
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
            cmd.Parameters.AddWithValue("@Brcid", Brcid);
            cmd.Parameters.AddWithValue("@Typeid", Typeid);
            cmd.Parameters.AddWithValue("@Vendid", Vendid);
            cmd.Parameters.AddWithValue("@Statid", Statid);
            cmd.Parameters.AddWithValue("@DateFilter", DateFilter);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            
            //cmd.Parameters.AddWithValue("@rtype", rtype);
            //cmd.Parameters.AddWithValue("@btype", btype);
            var response = new List<AssetModel>();
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

        //public async Task<List<AssetModel>> GetAllAssets_Paginated(int pageNumber,int pageSize)
        //{
        //    using SqlConnection sql = new(_connectionString);
        //    using SqlCommand cmd = new("sp_GetAllAssets", sql);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //    cmd.Parameters.AddWithValue("@PageSize", pageSize);

        //    var response = new List<AssetModel>();
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
            public async Task<List<AssetModel>> GetById(AssetModel a)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_SearchAllAssets_Paginated", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", a.Assetid));

            var response = new List<AssetModel>();
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
        public async Task<List<AssetModel>> GetId(int id)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_SearchAllAssets_Paginated", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            var response = new List<AssetModel>();
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

        public async Task Insert(AssetModel asset)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_AssetCreate", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@SerialNo", asset.SerialNo));
            cmd.Parameters.Add(new SqlParameter("@Branch", asset.Branch));
            cmd.Parameters.Add(new SqlParameter("@Brand", asset.Brand));
            cmd.Parameters.Add(new SqlParameter("@Type", asset.Type));
            cmd.Parameters.Add(new SqlParameter("@Model", asset.Model));
            cmd.Parameters.Add(new SqlParameter("@Processor_Type", asset.Processor_Type));
            cmd.Parameters.Add(new SqlParameter("@Monitor_Type", asset.Monitor_Type));
            cmd.Parameters.Add(new SqlParameter("@Range_Type", asset.Range_Type));
            cmd.Parameters.Add(new SqlParameter("@Battery_Type", asset.Battery_Type));
            cmd.Parameters.Add(new SqlParameter("@Battery_Ampere", asset.Battery_Ampere));
            cmd.Parameters.Add(new SqlParameter("@Battery_Capacity", asset.Battery_Capacity));
            cmd.Parameters.Add(new SqlParameter("@GraphicsCard", asset.GraphicsCard));
            cmd.Parameters.Add(new SqlParameter("@Optical_Drive", asset.Optical_Drive));
            cmd.Parameters.Add(new SqlParameter("@HDD", asset.HDD));
            cmd.Parameters.Add(new SqlParameter("@RAM", asset.RAM));
            cmd.Parameters.Add(new SqlParameter("@Inches", asset.Inches));
            cmd.Parameters.Add(new SqlParameter("@Port_Switch", asset.Port_Switch));
            cmd.Parameters.Add(new SqlParameter("@LastAllocated_To", asset.LastAllocated_To));
            cmd.Parameters.Add(new SqlParameter("@specification", asset.Specification));
            cmd.Parameters.Add(new SqlParameter("@Vendorid", asset.Vendorid));
            cmd.Parameters.Add(new SqlParameter("@Status", asset.Status));
            cmd.Parameters.Add(new SqlParameter("@Invoice_No", asset.Invoice_No));
            cmd.Parameters.Add(new SqlParameter("@Location", asset.Location));
            cmd.Parameters.Add(new SqlParameter("@Uid", asset.Uid));
            cmd.Parameters.Add(new SqlParameter("@Remarks", asset.Remarks));
            cmd.Parameters.Add(new SqlParameter("@Created_at", asset.Created_at));
            cmd.Parameters.Add(new SqlParameter("@InvoiceDate", asset.InvoiceDate));
            cmd.Parameters.Add(new SqlParameter("@Warranty_Till", asset.Warranty_Till));
            cmd.Parameters.Add(new SqlParameter("@Allocated_At", asset.Allocated_At));
            cmd.Parameters.Add(new SqlParameter("@active", 1));

            var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(returncode);
            var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(returnpart);

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
            bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;

            Itexists = itExists;
          IsSuccess = isSuccess;
            return;
        }
        #region commented code
        //public async Task<IEnumerable<AssetModel>> GetAll_Paginated(AssetPages ownerParameters)
        //{
        //    var owners = new List<AssetModel>();
        //    // Retrieve all owners from the data source, for example from a database
        //    using (var connection = new SqlConnection("connection_string"))
        //    {
        //        await connection.OpenAsync();
        //        using (var command = new SqlCommand("SELECT * FROM Assets", connection))
        //        {
        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    owners.Add(new AssetModel
        //                    {
        //                        Assetid = (int)reader["Assetid"],
        //                        SerialNo = reader["SerialNo"].ToString(),
        //                        Branch = (int)reader["Branch"],
        //                        Brand = reader["Brand"].ToString(),
        //                        Type = (int)reader["Type"],
        //                        Model = reader["Model"].ToString(),
        //                        Processor_Type = reader["Processor_Type"].ToString(),

        //                        Monitor_Type = reader["Monitor_Type"].ToString(),
        //                        Range_Type = reader["Range_Type"].ToString(),
        //                        Battery_Type = reader["Battery_Type"].ToString(),
        //                        Battery_Ampere = reader["Battery_Amperer"].ToString(),
        //                        Battery_Capacity = reader["Battery_Capacity"].ToString(),
        //                        GraphicsCard = reader["GraphicsCard"].ToString(),
        //                        Optical_Drive = reader["Optical_Drive"].ToString(),
        //                        HDD = reader["HDD"].ToString(),
        //                        RAM = reader["RAM"].ToString(),
        //                        Inches = reader["Inches"].ToString(),
        //                        Port_Switch = reader["Port_Switch"].ToString(),
        //                        Nos = (int)reader["Nos"],
        //                        Specification = reader["Specification"].ToString(),
        //                        Vendorid = (int)reader["Vendorid"],
        //                        Status = (int)reader["Status"],
        //                        Allocated_to = (int)reader["Range_Type"],
        //                        Remarks=reader["Remarks"].ToString(),
        //                        Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,

        //                        active = (bool)reader["active"],
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return owners
        //        .OrderBy(on => on.Assetid)
        //        .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
        //        .Take(ownerParameters.PageSize);
        //}
        //public async Task GetbyObj(CompanyMaster user, bool isAuth, bool Admin)
        //{
        //    try
        //    {
        //        using (SqlConnection sql = new SqlConnection(_connectionString))
        //        {
        //            await sql.OpenAsync();

        //            using (var command = new SqlCommand("login_user", sql))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@UserName", user.Email);
        //                command.Parameters.AddWithValue("@Password", user.Password);

        //                var isAuthenticatedParam = new SqlParameter("@isAuthenticated", SqlDbType.Bit) { Direction = ParameterDirection.Output };
        //                command.Parameters.Add(isAuthenticatedParam);
        //                var isAdmin = new SqlParameter("@isAdmin", SqlDbType.Bit) { Direction = ParameterDirection.Output };
        //                command.Parameters.Add(isAdmin);
        //                await sql.OpenAsync();
        //                await command.ExecuteNonQueryAsync();
        //                sql.Close();
        //                isAuth = (bool)isAuthenticatedParam.Value;
        //                Admin = (bool)isAdmin.Value;

        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}
        #endregion 
        public async Task Update([FromBody] AssetModel asset)
        {
            
                using SqlConnection sql = new(_connectionString);
                using SqlCommand cmd = new("sp_AssetCreate", sql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", asset.Assetid));
                cmd.Parameters.Add(new SqlParameter("@SerialNo", asset.SerialNo));
                cmd.Parameters.Add(new SqlParameter("@Branch", asset.Branch));
                cmd.Parameters.Add(new SqlParameter("@Brand", asset.Brand));
                cmd.Parameters.Add(new SqlParameter("@Type", asset.Type));
                cmd.Parameters.Add(new SqlParameter("@Model", asset.Model));
                cmd.Parameters.Add(new SqlParameter("@Processor_Type", asset.Processor_Type));
                cmd.Parameters.Add(new SqlParameter("@Monitor_Type", asset.Monitor_Type));
                cmd.Parameters.Add(new SqlParameter("@Range_Type", asset.Range_Type));
                cmd.Parameters.Add(new SqlParameter("@Battery_Type", asset.Battery_Type));
                cmd.Parameters.Add(new SqlParameter("@Battery_Ampere", asset.Battery_Ampere));
                cmd.Parameters.Add(new SqlParameter("@Battery_Capacity", asset.Battery_Capacity));
                cmd.Parameters.Add(new SqlParameter("@GraphicsCard", asset.GraphicsCard));
                cmd.Parameters.Add(new SqlParameter("@Optical_Drive", asset.Optical_Drive));
                cmd.Parameters.Add(new SqlParameter("@HDD", asset.HDD));
                cmd.Parameters.Add(new SqlParameter("@RAM", asset.RAM));
                cmd.Parameters.Add(new SqlParameter("@Inches", asset.Inches));
                cmd.Parameters.Add(new SqlParameter("@Port_Switch", asset.Port_Switch));
                cmd.Parameters.Add(new SqlParameter("@LastAllocated_To", asset.LastAllocated_To));
                cmd.Parameters.Add(new SqlParameter("@specification", asset.Specification));
                cmd.Parameters.Add(new SqlParameter("@Vendorid", asset.Vendorid));
                cmd.Parameters.Add(new SqlParameter("@Status", asset.Status));
                cmd.Parameters.Add(new SqlParameter("@Invoice_No", asset.Invoice_No));
                cmd.Parameters.Add(new SqlParameter("@Location", asset.Location));
                cmd.Parameters.Add(new SqlParameter("@Uid", asset.Uid));
                cmd.Parameters.Add(new SqlParameter("@Remarks", asset.Remarks));
                cmd.Parameters.Add(new SqlParameter("@Created_at", asset.Created_at));
                cmd.Parameters.Add(new SqlParameter("@InvoiceDate", asset.InvoiceDate));
                cmd.Parameters.Add(new SqlParameter("@Warranty_Till", asset.Warranty_Till));
                cmd.Parameters.Add(new SqlParameter("@Allocated_At", asset.Allocated_At));
                cmd.Parameters.Add(new SqlParameter("@active", 1));

                //var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                //cmd.Parameters.Add(returncode);
                var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(returnpart);
               

                await sql.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

               // bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
                bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;

              //  Itexists = itExists;
                IsSuccess = isSuccess;
                return;
           
        }
        public async Task Transfer( int id, int Branch, string Description)
        {

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_AssetTransfer", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", id));
            //cmd.Parameters.Add(new SqlParameter("@SerialNo", asset.SerialNo));
            cmd.Parameters.Add(new SqlParameter("@Branch", Branch));
            //-----------------------------------------------------------------------------
            //cmd.Parameters.Add(new SqlParameter("@TrfBranch", TrfBranch));
           // cmd.Parameters.Add(new SqlParameter("@Qty", asset.Qty));
            cmd.Parameters.Add(new SqlParameter("@Description",Description));
            //-----------------------------------------------------------------------------   
            var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(returncode);
            var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(returnpart);

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
            bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;

            Itexists = itExists;
            IsSuccess = isSuccess;
            return;
        }
        public async Task DeleteById(int id)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_DeleteAssets", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Id", id));
            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return;
        }
    }
}
