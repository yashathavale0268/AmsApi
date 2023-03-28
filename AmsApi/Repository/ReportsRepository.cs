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


        internal async Task<DataSet> GetisinuseReport(int inuse)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAssetType_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@typ", inuse);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return await Task.FromResult(dataSet);
            }
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
        //int pageNumber, int pageSize, string searchString, int brcid
        public DataSet GetInUseTable()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllinuseSpecific_total_Report ", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                //        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                //cmd.Parameters.AddWithValue("@SearchString", searchString);
                //cmd.Parameters.AddWithValue("@brcid ", brcid);
                
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }
        public DataSet GetIsSpareTable()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllisSpareSpecific_total_Report", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }
        public DataSet GetIsWorkingTable()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllisWorkingSpecific_total_Report ", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }
    }
}
