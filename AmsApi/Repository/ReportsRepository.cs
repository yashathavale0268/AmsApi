using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using Microsoft.Extensions.Configuration;

namespace AmsApi.Repository
{
    public class ReportsRepository
    {
        private readonly string _connectionString;
        public ReportsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }

        internal async Task<DataSet> GetDashbordValues()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllReportqueries", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return await Task.FromResult(dataSet);
            }
        }
        internal async Task<DataSet> GetAssetReport(string reportType, int brch, int comp, int dep, int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetSpecificTotalReport", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@reportType", reportType);
                cmd.Parameters.AddWithValue("@brch", brch);
                cmd.Parameters.AddWithValue("@comp", comp);
                cmd.Parameters.AddWithValue("@dep", dep);
                cmd.Parameters.AddWithValue("@typ", typ);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return await Task.FromResult(dataSet); 
            }
        }
        internal async Task<DataSet> GetBranchviseReport(int brch,int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetBranchSpecific_total_Report ", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@brch", brch);
                cmd.Parameters.AddWithValue("@typ", typ);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return await Task.FromResult(dataSet);
            }
        }

        internal async Task<DataSet> GetCompanyviseReport(int comp,int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetCompanySpecific_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@comp", comp);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return await Task.FromResult(dataSet);
            }
        }

        internal async Task<DataSet> GetDepartmentviseReport(int dep,int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetDepartmentSpecific_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dep", dep);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return await Task.FromResult(dataSet);
            }
        }

        internal async Task<DataSet> GetisworkingReport(int isworking)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAssetType_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@typ", isworking);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return await Task.FromResult(dataSet);
            }
        }


        internal async Task<List<ReportModel>> GetisinuseReport()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAssetType_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                var response = new List<ReportModel>();
                
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue_inUse(reader));
                    }
                }

                return response;
            }
        }
        public ReportModel MapToValue_inUse(SqlDataReader reader)
        {
            return new ReportModel()
            {
                userinfo = (string)reader["userinfo"],
                branch = (string)reader["branch"],

                type = (string)reader["type"],

                assetsinUse = (int)reader["assetsinUse"],

                Statid = (int)reader["Statid"],
                status = (string)reader["status"],
                totalrecord = (int)reader["totalrecord"],
            };
        }
        public ReportModel MapToValue_isSpare(SqlDataReader reader)
        {
            return new ReportModel()
            {
                userinfo = (string)reader["userinfo"],
                branch = (string)reader["branch"],

                type = (string)reader["type"],

                

                spareassets = (int)reader["spareassets"],


                Statid = (int)reader["Statid"],
                status = (string)reader["status"],
                totalrecord = (int)reader["totalrecord"],
            };
        }
        public ReportModel MapToValue_isWorking(SqlDataReader reader)
        {
            return new ReportModel()
            {
                userinfo = (string)reader["userinfo"],
                branch = (string)reader["branch"],

                type = (string)reader["type"],

                workingassets = (int)reader["workingassets"],
                Statid = (int)reader["Statid"],
                status = (string)reader["status"],
                totalrecord = (int)reader["totalrecord"],

            };
        }
        public ReportModel MapToValue_New_Request(SqlDataReader reader)
        {
            return new ReportModel()
            {
                userinfo = (string)reader["userinfo"],
                branch = (string)reader["branch"],

                type = (string)reader["type"],
                AssetsRequested = (int)reader["AssetsRequested"],
                
                Statid = (int)reader["Statid"],
                status = (string)reader["status"],
                totalrecord = (int)reader["totalrecord"],

            };
        }

        internal async Task<DataSet> GetscrapReport(int srp)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAssetType_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@typ", srp);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return await Task.FromResult(dataSet);
            }
        }

        public DataSet GetReportTable()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetSpecific_total_Report ", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }
        //
        public async Task<List<ReportModel>> GetInUseTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllinuseSpecific_total_Report ", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pagenumber", pageNumber);
                cmd.Parameters.AddWithValue("@pagesize", pageSize);
                cmd.Parameters.AddWithValue("@searchstring", searchString);
                cmd.Parameters.AddWithValue("@brcid", brcid);
                cmd.Parameters.AddWithValue("@typ", typ);
                var response = new List<ReportModel>();
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue_inUse(reader));
                    }
                }

                return response;
            }
        }
        public async Task<List<ReportModel>> GetIsSpareTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllisSpareSpecific_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pagenumber", pageNumber);
                cmd.Parameters.AddWithValue("@pagesize", pageSize);
                cmd.Parameters.AddWithValue("@searchstring", searchString);
                cmd.Parameters.AddWithValue("@brcid", brcid);
                cmd.Parameters.AddWithValue("@typ", typ);

                var response = new List<ReportModel>();
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue_isSpare(reader));
                    }
                }

                return response;
            }
        }
        public async Task<List<ReportModel>> GetIsWorkingTable(int pageNumber, int pageSize, string searchString, int brcid, int typ, int stat)//int pageNumber,int pageSize, string SearchString, int brcid, int typ, int stat
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllisWorkingSpecific_total_Report ", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pagenumber", pageNumber);
                cmd.Parameters.AddWithValue("@pagesize", pageSize);
                cmd.Parameters.AddWithValue("@SearchString", searchString);
                cmd.Parameters.AddWithValue("@brcid", brcid);
                cmd.Parameters.AddWithValue("@typ", typ);
                cmd.Parameters.AddWithValue("@stat", stat);
                var response = new List<ReportModel>();
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue_isWorking(reader));
                    }
                }

                return response;
            }
        }
        public async Task<List<ReportModel>> GetNew_RequestTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)//int pageNumber, int pageSize, string SearchString, int brcid, int typ
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllNew_RequestSpecific_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pagenumber", pageNumber);
                cmd.Parameters.AddWithValue("@pagesize", pageSize);
                cmd.Parameters.AddWithValue("@searchstring", searchString);
                cmd.Parameters.AddWithValue("@brcid", brcid);
                cmd.Parameters.AddWithValue("@typ", typ);
                var response = new List<ReportModel>();
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue_New_Request(reader));
                    }
                }

                return response;
            }
        }
    }
}
