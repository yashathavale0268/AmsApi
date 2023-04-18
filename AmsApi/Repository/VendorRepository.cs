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
    public class VendorRepository
    {
        private readonly string _connectionString;
        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }
        public VendorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }
        //internal async Task<List<VendorModel>> GetAllVendors()
        //{
        //    using (SqlConnection sql = new(_connectionString))
        //    {
        //        using (SqlCommand cmd = new("sp_GetAllVendors", sql))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            var response = new List<VendorModel>();
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

        internal async Task<List<VendorModel>> GetAllVendors(int pageNumber, int pageSize)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllVendors", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    var response = new List<VendorModel>();
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

        

        internal async Task<List<VendorModel>> GetById(VendorModel v)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllVendors_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id",v.Vendorid));
                    // VendorModel response = null;
                    var response = new List<VendorModel>();
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
        internal async Task<List<VendorModel>> GetId(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllVendors_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    // VendorModel response = null;
                    var response = new List<VendorModel>();
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
        internal async Task<List<VendorModel>> GetDetId(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllVendors_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    // VendorModel response = null;
                    var response = new List<VendorModel>();
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

        internal async Task<List<VendorModel>> SearchVendors(int pageNumber, int pageSize, string searchTerm, string StartDate ,string EndDate,int DateFilter)
        {

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchAllVendors_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                         cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@DateFilter", DateFilter);
                    var response = new List<VendorModel>();
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

        public VendorModel MapToValue(SqlDataReader reader)
        {
            return new VendorModel()
            {
                Vendorid = (int)reader["Vendorid"],
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? "N/A" : (string)reader["Name"],//.ToString()
                InvoiceNo = reader.IsDBNull(reader.GetOrdinal("InvoiceNo")) ? "N/A" : (string)reader["InvoiceNo"],//.ToString()
                //InvoiceDate = (reader["InvoiceDate"] != DBNull.Value) ? Convert.ToDateTime(reader["InvoiceDate"]) : DateTime.MinValue,
                //Warranty_Till = (reader["Warranty_Till"] != DBNull.Value) ? Convert.ToDateTime(reader["Warranty_Till"]) : DateTime.MinValue,
                active = (bool)reader["active"],
                Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"]
            };
        }

        public async Task Insert(VendorModel vendor)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_VendorCreate", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //  cmd.Parameters.AddWithValue("@id", comp.Companyid);
                    cmd.Parameters.AddWithValue("@Name", vendor.Name);
                    cmd.Parameters.AddWithValue("@InvoiceNo", vendor.InvoiceNo);
                    //cmd.Parameters.AddWithValue("@InvoiceDate", vendor.InvoiceDate);
                    //cmd.Parameters.AddWithValue("@Warranty_Till", vendor.Warranty_Till);
                    cmd.Parameters.AddWithValue("@active", 1);

                    await sql.OpenAsync();
                    var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returncode);
                    var returnpart = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returnpart);
                    await cmd.ExecuteNonQueryAsync();
                    await sql.CloseAsync();
                    bool itExists = returncode?.Value is not DBNull && (bool)returncode.Value;
                    bool isSuccess = returnpart?.Value is not DBNull && (bool)returnpart.Value;
                    Itexists = itExists;
                    IsSuccess = isSuccess;
                    
                    return;
                }
            }
        }
              public async Task DeletevendorById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_DeleteVendor", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task UpdateVendor( VendorModel vendor)
        {
            try
            {
                using (SqlConnection sql = new(_connectionString))
                {
                    using (SqlCommand cmd = new("sp_VendorCreate", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", vendor.Vendorid);
                        cmd.Parameters.AddWithValue("@Name", vendor.Name);
                        cmd.Parameters.AddWithValue("@InvoiceNo", vendor.InvoiceNo);
                        //cmd.Parameters.AddWithValue("@InvoiceDate", vendor.InvoiceDate);
                        //cmd.Parameters.AddWithValue("@Warranty_Till", vendor.Warranty_Till);
                        cmd.Parameters.AddWithValue("@active", 1);
                        
                      //  var returncode = new SqlParameter("@Exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
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
