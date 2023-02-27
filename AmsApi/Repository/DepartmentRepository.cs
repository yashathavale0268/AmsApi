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
    public class DepartmentRepository
    {
        private readonly string _connectionString;
        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }
        public DepartmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        public async Task<List<DepartmentModel>> GetAll()
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_GetAllDep", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var response = new List<DepartmentModel>();
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

        internal async Task<List<DepartmentModel>> GetAllDep(int pageNumber, int pageSize)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllDep", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            var response = new List<DepartmentModel>();
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

        internal async Task<List<DepartmentModel>> SearchDepartment(int pageNumber, int pageSize, string searchTerm,int dep)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchAllDep_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                    cmd.Parameters.AddWithValue("@Dep", dep);
                    var response = new List<DepartmentModel>();
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

        public DepartmentModel MapToValue(SqlDataReader reader)
        {
            return new DepartmentModel()
            {
                Depid = (int)reader["Depid"],
                Name = reader["Name"].ToString(),
                Remarks= reader["Remarks"].ToString(),
                Created_at = (DateTime)reader["Created_at"],
                active = (bool)reader["active"],
            };
        }

        public async Task<List<DepartmentModel>> GetById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_GetAllDep", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    var response = new List<DepartmentModel>();
                    //DepartmentModel response = null;
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

        internal async Task UpdateDep(DepartmentModel dep, int id)
        {
            try
            {
                using (SqlConnection sql = new(_connectionString))
                {
                    using (SqlCommand cmd = new("sp_DepartmentCreate", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@Name", dep.Name);
                        cmd.Parameters.AddWithValue("@Remarks", dep.Remarks);
                        cmd.Parameters.AddWithValue("@Created_at", dep.Created_at);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task Insert(DepartmentModel dep)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_DepartmentCreate", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Name", dep.Name));
                    cmd.Parameters.Add(new SqlParameter("@Remarks", dep.Remarks));
                    cmd.Parameters.Add(new SqlParameter("@Created_at", dep.Created_at));
                    cmd.Parameters.Add(new SqlParameter("@active", 1));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }
        #region old code
        //public async Task GetbyObj(DepartmentMaster user, bool isAuth, bool Admin)
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

        public async Task Update([FromBody] DepartmentModel dep, int id)
        {
            try {
                using (SqlConnection sql = new(_connectionString))
                {
                    using (SqlCommand cmd = new("sp_DeparmentCreate", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@Name", dep.Name));
                        cmd.Parameters.Add(new SqlParameter("@Remarks", dep.Remarks));
                        cmd.Parameters.Add(new SqlParameter("@Created_at", dep.Created_at));
                        cmd.Parameters.Add(new SqlParameter("@active", 1));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return ;
                    }
                }
            }
            catch(Exception)
            {
                
            }
        }

        public async Task DeleteById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_DeleteDep", sql))
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
