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
        internal async Task<DataSet> GetAssetReport( int brch , int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetSpecific_total_Report", sql);
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
        internal async Task<List<ReportModel>> GetQty()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetReportQty", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                var response = new List<ReportModel>();

                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue_qty(reader));
                    }
                }

                return response;
            }
        }

        //internal async Task<DataSet> GetScrapsTable(int brch, int typ)
        //{
            
        //    using SqlConnection sql = new(_connectionString);
        //    using SqlCommand cmd = new("sp_GetSpecific_total_Report", sql);
        //    {

        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@brch", brch);

        //        cmd.Parameters.AddWithValue("@typ", typ);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        DataSet dataSet = new DataSet();
        //        adapter.Fill(dataSet);

        //        return await Task.FromResult(dataSet);
        //    }
        //}
        private ReportModel MapToValue_qty(SqlDataReader reader)
        {
            return new ReportModel()
            {
               
                brchid = reader.IsDBNull(reader.GetOrdinal("branch")) ? 0 : (int)reader["branch"],

                typid = reader.IsDBNull(reader.GetOrdinal("type")) ? 0 : (int)reader["type"],

                locid= reader.IsDBNull(reader.GetOrdinal("locid")) ? 0 : (int)reader["locid"],

                Statid = reader.IsDBNull(reader.GetOrdinal("Status")) ? 0 : (int)reader["Status"],
                Qty= reader.IsDBNull(reader.GetOrdinal("Qty")) ? 0 : (int)reader["Qty"]
            };
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
                userinfo = reader.IsDBNull(reader.GetOrdinal("userinfo")) ? null : (string)reader["userinfo"],
                branch = reader.IsDBNull(reader.GetOrdinal("branch")) ? null : (string)reader["branch"],

                type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : (string)reader["type"],

                assetsinUse = reader.IsDBNull(reader.GetOrdinal("assetsinUse")) ? 0 : (int)reader["assetsinUse"],

                Statid = reader.IsDBNull(reader.GetOrdinal("Statid")) ? 0 : (int)reader["Statid"],
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : (string)reader["status"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"],
            };
        }
        public ReportModel MapToValue_isSpare(SqlDataReader reader)
        {
            return new ReportModel()
            {
                //userinfo = (string)reader["userinfo"],
                branch = reader.IsDBNull(reader.GetOrdinal("branch")) ? null : (string)reader["branch"],

                type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : (string)reader["type"],

                

                spareassets = reader.IsDBNull(reader.GetOrdinal("spareassets")) ? 0 : (int)reader["spareassets"],


                Statid = reader.IsDBNull(reader.GetOrdinal("Statid")) ? 0 : (int)reader["Statid"],
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : (string)reader["status"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"],
            };
        }
        public ReportModel MapToValue_isWorking(SqlDataReader reader)
        {
            return new ReportModel()
            {
                //userinfo = (string)reader["userinfo"],
                branch = reader.IsDBNull(reader.GetOrdinal("branch")) ? null : (string)reader["branch"],

                type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : (string)reader["type"],

                workingassets = reader.IsDBNull(reader.GetOrdinal("workingassets")) ? 0 : (int)reader["workingassets"],
                Statid = reader.IsDBNull(reader.GetOrdinal("Statid")) ? 0 : (int)reader["Statid"],
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : (string)reader["status"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"],

            };
        }

        internal async Task<List<ReportModel>> GetReadyForScrapTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllScrapsSpecific_total_Report", sql);
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
                        response.Add(MapToValue_ReadyForScrap(reader));
                    }
                }

                return response;
            }
        }

        public ReportModel MapToValue_ReadyForScrap(SqlDataReader reader)
        {
            return new ReportModel()
            {
                //userinfo = (string)reader["userinfo"],
                branch = reader.IsDBNull(reader.GetOrdinal("branch")) ? null : (string)reader["branch"],

                type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : (string)reader["type"],



                ReadyForScrap = reader.IsDBNull(reader.GetOrdinal("ReadyForScrap")) ? 0 : (int)reader["ReadyForScrap"],


                Statid = reader.IsDBNull(reader.GetOrdinal("Statid")) ? 0 : (int)reader["Statid"],
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : (string)reader["status"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"],
            };
        }

        public ReportModel MapToValue_InProcess(SqlDataReader reader)
        {
            return new ReportModel()
            {
                userinfo = reader.IsDBNull(reader.GetOrdinal("userinfo")) ? null : (string)reader["userinfo"],
                branch = reader.IsDBNull(reader.GetOrdinal("branch")) ? null : (string)reader["branch"],

                type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : (string)reader["type"],
                AssetsRequested = reader.IsDBNull(reader.GetOrdinal("AssetsRequested")) ? 0 : (int)reader["AssetsRequested"],
                
                Statid = reader.IsDBNull(reader.GetOrdinal("Statid")) ? 0 : (int)reader["Statid"],
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : (string)reader["status"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"],

            };
        }
        public ReportModel MapToValue_isScraped(SqlDataReader reader)
        {
            return new ReportModel()
            {
                //userinfo = (string)reader["userinfo"],
                branch = reader.IsDBNull(reader.GetOrdinal("branch")) ? null : (string)reader["branch"],

                type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : (string)reader["type"],



                scrapedassets = reader.IsDBNull(reader.GetOrdinal("ScrapedAssets")) ? 0 : (int)reader["ScrapedAssets"],


                Statid = reader.IsDBNull(reader.GetOrdinal("Statid")) ? 0 : (int)reader["Statid"],
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : (string)reader["status"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"],
            };
        }

        public ReportModel MapToValue_SentForFix(SqlDataReader reader)
        {
            return new ReportModel()
            {
                //userinfo = (string)reader["userinfo"],
                branch = reader.IsDBNull(reader.GetOrdinal("branch")) ? null : (string)reader["branch"],

                type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : (string)reader["type"],



                SentForFix = reader.IsDBNull(reader.GetOrdinal("SentForFix")) ? 0 : (int)reader["SentForFix"],


                Statid = reader.IsDBNull(reader.GetOrdinal("Statid")) ? 0 : (int)reader["Statid"],
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : (string)reader["status"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"],
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
            using SqlCommand cmd = new("sp_GetAll_Table_total_Report", sql);
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
        public async Task<List<ReportModel>> GetInProcessTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)//int pageNumber, int pageSize, string SearchString, int brcid, int typ
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllInProcessSpecific_total_Report", sql);
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
                        response.Add(MapToValue_InProcess(reader));
                    }
                }

                return response;
            }
        }
       
        public async Task<List<ReportModel>> GetScrapsTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllScrapsSpecific_total_Report", sql);
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
                        response.Add(MapToValue_isScraped(reader));
                    }
                }

                return response;
            }
        }

        public async Task<List<ReportModel>> GetScrapTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllReadyForScrapSpecific_total_Report", sql);
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
                        response.Add(MapToValue_isScraped(reader));
                    }
                }

                return response;
            }
        }

        public async Task<List<ReportModel>> GetAllSentForFixTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllMaintenanceSpecific_total_Report", sql);
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
                        response.Add(MapToValue_isScraped(reader));
                    }
                }

                return response;
            }
        }

    }
}
