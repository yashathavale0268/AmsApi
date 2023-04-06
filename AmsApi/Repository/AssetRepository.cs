using System;
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
        public AssetModel MapToValue(SqlDataReader reader)
        {
            return new AssetModel() { 
            Assetid = (int)reader["Assetid"],
                SerialNo = reader["SerialNo"].ToString(),
                Branch =(int)reader["Branch"],
                Branches =(string)reader["Branches"],
                Brand =reader["Brand"].ToString(),
                Type = (int)reader["Type"],
                TypeName =(string)reader["TypeName"],
                Model =reader["Model"].ToString(),
                Processor_Type=reader["Processor_Type"].ToString(),
               
                Monitor_Type= reader["Monitor_Type"].ToString(),
                Range_Type=reader["Range_Type"].ToString(),
                Battery_Type= reader["Battery_Type"].ToString(),
                Battery_Ampere=reader["Battery_Ampere"].ToString(),
                Battery_Capacity=reader["Battery_Capacity"].ToString(),
                GraphicsCard=reader["GraphicsCard"].ToString(),
                Optical_Drive=reader["Optical_Drive"].ToString(),
                HDD=reader["HDD"].ToString(),
                RAM=reader["RAM"].ToString(),
                Inches=reader["Inches"].ToString(),
                Port_Switch=reader["Port_Switch"].ToString(),
                Nos=(int)reader["Nos"],
                Specification=reader["Specification"].ToString(),
                Vendorid=(int)reader["Vendorid"],
                Vendors = (string)reader["Vendors"],
                //Status =(int)reader["Status"],
                //Statuses = (string)reader["Statuses"],
                //Allocated_to=(int)reader["Allocated_to"],

                Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
              
            active = (bool)reader["active"],
                totalrecord = (int)reader["totalrecord"]
                
                
               
               // Lastuser = reader["Lastuser"].ToString(),
            };
        }

        public async Task<List<AssetModel>> SearchAssets(int pageNumber, int pageSize, string searchTerm, int Brcid ,int Typeid )//,string ptype,string mtype,string rtype , string btype )
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
          
            //cmd.Parameters.AddWithValue("@ptype", ptype);
            //cmd.Parameters.AddWithValue("@mtype", mtype);
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
            public async Task<List<AssetModel>> GetById(int id)
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
            cmd.Parameters.Add(new SqlParameter("@Nos", asset.Nos));
            cmd.Parameters.Add(new SqlParameter("@specification", asset.Specification));
            cmd.Parameters.Add(new SqlParameter("@Vendorid", asset.Vendorid));
            //cmd.Parameters.Add(new SqlParameter("@Status", asset.Status));
            //cmd.Parameters.Add(new SqlParameter("@Allocated_to", asset.Allocated_to));
            cmd.Parameters.Add(new SqlParameter("@Remarks", asset.Remarks));
            cmd.Parameters.Add(new SqlParameter("@Created_at", asset.Created_at));
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
        public async Task Update([FromBody] AssetModel asset, int id)
        {
            try {
              
                using SqlConnection sql = new(_connectionString);
                using SqlCommand cmd = new("sp_AssetCreate", sql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));
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
                cmd.Parameters.Add(new SqlParameter("@Nos", asset.Nos));
                cmd.Parameters.Add(new SqlParameter("@specification", asset.Specification));
                cmd.Parameters.Add(new SqlParameter("@Vendorid", asset.Vendorid));
                //cmd.Parameters.Add(new SqlParameter("@Status", asset.Status));
                //cmd.Parameters.Add(new SqlParameter("@Allocated_to", asset.Allocated_to));
                cmd.Parameters.Add(new SqlParameter("@Created_at", asset.Created_at));
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
            catch(Exception)
            {

            }
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
