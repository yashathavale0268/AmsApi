using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AmsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AmsApi.Repository
{
    public class AssettypeRepository
    {
        private readonly string _connectionString;

       
        public bool Itexists { get;   set; }
        public bool IsSuccess { get;  set; }

        public AssettypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        public async Task<List<AssettypeModel>> GetAlltypes()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllTypes", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            var response = new List<AssettypeModel>();
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

        internal async Task<List<AssettypeModel>> GetAllTypes(int pageNumber, int pageSize)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllTypes", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            var response = new List<AssettypeModel>();
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

        internal async Task<List<AssettypeModel>> SearchTypes(int pageNumber, int pageSize, string searchTerm,int typeid)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_SearchAllType_Paginated", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
            cmd.Parameters.AddWithValue("@typeid", typeid);
            var response = new List<AssettypeModel>();
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

        public AssettypeModel MapToValue(SqlDataReader reader)
        {
            return new AssettypeModel()
            {
                Typeid = (int)reader["Typeid"],
                Name = reader["Name"].ToString(),
                Remarks = reader["Remarks"].ToString(),
                Active = (bool)reader["active"],
            };
        }

        public async Task<List<AssettypeModel>> GettypeById(int Id)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllTypes", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", Id);

            var response = new List<AssettypeModel>();
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

        public async Task Insert(AssettypeModel type)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_AssettypeCreate", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Name", type.Name));
            cmd.Parameters.Add(new SqlParameter("@Remarks", type.Remarks));
            cmd.Parameters.Add(new SqlParameter("@active", 1));

            var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(returncode);
            var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(returnpart);
            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await sql.CloseAsync();

            bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
            bool  isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
            Itexists = itExists;
            IsSuccess = isSuccess;
            return;
        }

        //public async Task GetbyObj(AssettypeMaster user, bool isAuth, bool Admin)
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

       
        public async Task UpdateType([FromBody] AssettypeModel type, int id)
        {
            try
            {
                using SqlConnection sql = new(_connectionString);
                await sql.OpenAsync();
                using SqlCommand cmd = new("sp_AssettypeCreate", sql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@Name", type.Name));
                cmd.Parameters.Add(new SqlParameter("@Remarks", type.Remarks));
                cmd.Parameters.Add(new SqlParameter("@active", type.Active));
               
                //var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
              //  cmd.Parameters.Add(returncode);
                var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(returnpart);
                
                await cmd.ExecuteNonQueryAsync();
              
              //  bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
                bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
              //  Itexists = itExists;
                IsSuccess = isSuccess;
                return;
            }
            catch (Exception)
            {

            }
        }

        public async Task DeletetypeById(int id)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_DeleteAssettype", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return;
        }
    }
}
