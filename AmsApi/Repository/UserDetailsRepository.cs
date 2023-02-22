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
    public class UserDetailsRepository
    {
        private readonly string _connectionString;
        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }
        public UserDetailsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        internal async Task<List<UserDetailsModel>> GetAll()
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_GetAllUserDetails", sql))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    var response = new List<UserDetailsModel>();
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

        internal async Task<List<UserDetailsModel>> GetAllDetails(int pageNumber, int pageSize)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllUserDetails", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            var response = new List<UserDetailsModel>();
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

        internal async Task<List<UserDetailsModel>> SearchUserDetails(int pageNumber, int pageSize, string searchTerm,int searchId,int depId,int brcId,int compId,int userId,int floor)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchAllUserDetails_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                    cmd.Parameters.AddWithValue("@Searchid", searchId);
                    cmd.Parameters.AddWithValue("@Depid", depId);
                    cmd.Parameters.AddWithValue("@Brcid", brcId);
                    cmd.Parameters.AddWithValue("@Compid", compId);
                    cmd.Parameters.AddWithValue("@Userid", userId);
                    cmd.Parameters.AddWithValue("@Floor ", floor);

                    var response = new List<UserDetailsModel>();
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

        public UserDetailsModel MapToValue(SqlDataReader reader)
        {
            return new UserDetailsModel()
                {
                    Empid = (int)reader["Empid"],
                    Users=(int)reader["Users"],
                    First_name=reader["First_name"].ToString(),
                    Last_name=reader["Last_name"].ToString(),
                    Department=(int)reader["Department"],
                    Branch=(int)reader["Branch"],
                    Floor=(int)reader["Floor"],
                    Company=(int)reader["Company"],
                    Created_at= (DateTime)reader["Created_at"],
                    active=(bool)reader["active"],
                };
            
        }

        public async Task<UserDetailsModel> GetById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_GetAllUserDetails", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                   UserDetailsModel response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValue(reader);
                        }
                    }

                    return response;
                }
            }
        }

        internal async Task UpdateUserDetails(UserDetailsModel details, int id)
        {
            try
            {
                using (SqlConnection sql = new(_connectionString))
                {
                    using (SqlCommand cmd = new("sp_UserDetailsCreate", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@User", details.Users);
                        cmd.Parameters.AddWithValue("@First_name", details.First_name);
                        cmd.Parameters.AddWithValue("@Last_name", details.Last_name);
                        cmd.Parameters.AddWithValue("@Dep", details.Department);
                        cmd.Parameters.AddWithValue("@Branch", details.Branch);
                        cmd.Parameters.AddWithValue("@Floor", details.Floor);
                        cmd.Parameters.AddWithValue("@Comp", details.Company);
                        cmd.Parameters.AddWithValue("@Created_at", details.Created_at);
                        cmd.Parameters.AddWithValue("@active", 1);
                        await sql.OpenAsync();

                     //   var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
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

        internal async Task Insert(UserDetailsModel details)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_UserDetailsCreate", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@id", comp.Companyid);
                    cmd.Parameters.AddWithValue("@User", details.Users);
                    cmd.Parameters.AddWithValue("@First_name", details.First_name);
                    cmd.Parameters.AddWithValue("@Last_name", details.Last_name);
                    cmd.Parameters.AddWithValue("@Dep", details.Department);
                    cmd.Parameters.AddWithValue("@Branch", details.Branch);
                    cmd.Parameters.AddWithValue("@Floor", details.Floor);
                    cmd.Parameters.AddWithValue("@Comp", details.Company);
                    cmd.Parameters.AddWithValue("@Created_at", details.Created_at);
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
                using (SqlCommand cmd = new("sp_DeleteUserDetails", sql))
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
