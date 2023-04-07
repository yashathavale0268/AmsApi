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
    public class BranchRepository
    {
        private readonly string _connectionString;

        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }

        public BranchRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        //public async Task<List<BranchModel>> GetAll(int pageNumber =1, int pageSize =5 )
        //{
        //    using SqlConnection sql = new(_connectionString);
        //    using (SqlCommand cmd = new("sp_GetAllBranch", sql))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //        var response = new List<BranchModel>();
        //        await sql.OpenAsync();

        //        using (var reader = await cmd.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                response.Add(MapToValue(reader));
        //            }
        //        }

        //        return response;
        //    }
        //}

        //internal async Task<List<BranchModel>> GetAllBranch(int pageNumber, int pageSize)
        //{
        //    using SqlConnection sql = new(_connectionString);
        //    using SqlCommand cmd = new("sp_GetAllBranch", sql);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //    cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //    var response = new List<BranchModel>();
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

        public async Task<List<BranchModel>> SearchBranches(int pageNumber, int pageSize, string searchTerm, int brcid )
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchAllBranches_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                    cmd.Parameters.AddWithValue("@brcid", brcid);
                    var response = new List<BranchModel>();
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

        public BranchModel MapToValue(SqlDataReader reader)
        {
            return new BranchModel()
            {
                Branchid = (int)reader["Branchid"],
                Name = reader["Name"].ToString(),
                Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                Active = (bool)reader["active"],
                totalrecord = (int)reader["totalrecord"],
            };
        }

        

        public async Task<List<BranchModel>> GetById(int id)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_SearchAllBranches_Paginated", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@brcid", id));

            var response = new List<BranchModel>();
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

        internal async Task<List<BranchModel>> GetBranchById(BranchModel b)
        {
            using SqlConnection sql = new(_connectionString);
           
            using (SqlCommand command = new("sp_SearchAllBranches_Paginated", sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@brcid",b.Branchid);

                var response = new List<BranchModel>();
                await sql.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue(reader));
                    }
                }
                await sql.CloseAsync();
                return response;
            }
        }
        internal async Task UpdateBranch( BranchModel branch)
        {
            using SqlConnection sql = new(_connectionString);
            await sql.OpenAsync();
            using SqlCommand command = new("sp_BranchCreate", sql);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id",branch.Branchid);
            command.Parameters.AddWithValue("@Name", branch.Name);
            command.Parameters.AddWithValue("@Created_at", branch.Created_at);

           // var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
           // command.Parameters.Add(returncode);
            var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            command.Parameters.Add(returnpart);
            await command.ExecuteNonQueryAsync();
            await sql.CloseAsync();
          //  bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
            bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
           // Itexists = itExists;
            IsSuccess = isSuccess;
            return;
        }

        public async Task Insert(BranchModel branch)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_BranchCreate", sql);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Name", branch.Name));
            cmd.Parameters.Add(new SqlParameter("@Created_at", branch.Created_at));
            cmd.Parameters.Add(new SqlParameter("@active", 1));
            var returncode = new SqlParameter("@exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(returncode);
            var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(returnpart);

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await sql.CloseAsync();
            bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
            bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
            Itexists = itExists;
            IsSuccess = isSuccess;
            return;
        }

        public async Task Update([FromBody] BranchModel branch)
        {
            try {
                using (SqlConnection sql = new(_connectionString))
                {
                    using SqlCommand cmd = new("sp_BranchCreate", sql);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", branch.Branchid);
                    cmd.Parameters.AddWithValue("@Name", branch.Name);
                    cmd.Parameters.AddWithValue("@Created_at", branch.Created_at);
                    cmd.Parameters.AddWithValue("@active", 1);

                   // var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                   // cmd.Parameters.Add(returncode);
                    var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returnpart);
                   
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await sql.CloseAsync();
                    // bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
                    bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
                  //  Itexists = itExists;
                    IsSuccess = isSuccess;
                    return;
                }
            }
            catch(Exception)
            {
                
            }
        }

        public async Task DeleteById(int id)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_DeleteBranch", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await sql.CloseAsync();
            return;
        }
    }
}
