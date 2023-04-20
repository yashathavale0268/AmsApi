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
    public class LocationRepository
    {
        private readonly string _connectionString;

        public bool Itexists { get; set; }
        public bool IsSuccess { get; set; }

        public LocationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
        }

        //internal async Task<List<LocationModel>> GetAllLocations_Paginated(int pageNumber, int pageSize, int lid, int aid, int tid, int uid, int bid, int cid, int did, int rid, int f,int stat)
        //{
        //    using SqlConnection sql = new(_connectionString);
        //    using SqlCommand cmd = new("sp_GetAllLocations", sql);
        //    {

        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //        cmd.Parameters.AddWithValue("@lid", lid);
        //        cmd.Parameters.AddWithValue("@aid", aid);
        //        cmd.Parameters.AddWithValue("@tid", tid);
        //        cmd.Parameters.AddWithValue("@uid", uid);
        //        cmd.Parameters.AddWithValue("@bid", bid);
        //        cmd.Parameters.AddWithValue("@cid", cid);
        //        cmd.Parameters.AddWithValue("@did", did);
        //        cmd.Parameters.AddWithValue("@rid", rid);
        //        cmd.Parameters.AddWithValue("@f", f);
        //        cmd.Parameters.AddWithValue("@stat", stat);
        //        var response = new List<LocationModel>();
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

        private LocationModel MapToValue(SqlDataReader reader)
        {
            return new LocationModel()
            {
                locid = (int)reader["locid"],
                uid = reader.IsDBNull(reader.GetOrdinal("uid")) ? 0: (int)reader["uid"],
                Asset = reader.IsDBNull(reader.GetOrdinal("Asset")) ? 0 : (int)reader["Asset"],
                type = reader.IsDBNull(reader.GetOrdinal("type")) ? 0 : (int)reader["type"],
                branch = reader.IsDBNull(reader.GetOrdinal("branch")) ? 0 : (int)reader["branch"],
                department = reader.IsDBNull(reader.GetOrdinal("department")) ? 0 : (int)reader["department"],
                company = reader.IsDBNull(reader.GetOrdinal("company")) ? 0 : (int)reader["company"],
                floor = reader.IsDBNull(reader.GetOrdinal("floor")) ? "N/A" : (string)reader["floor"],
                reqid = reader.IsDBNull(reader.GetOrdinal("reqid")) ? 0 : (int)reader["reqid"],
                Extradetails = reader.IsDBNull(reader.GetOrdinal("Extradetails")) ? "N/A" : (string)reader["Extradetails"],
                active = (bool)reader["active"],
                status = reader.IsDBNull(reader.GetOrdinal("status")) ? 0 : (int)reader["status"],
                Assetname = reader.IsDBNull(reader.GetOrdinal("Assetname")) ? "N/A" : (string)reader["Assetname"],
                branchname= reader.IsDBNull(reader.GetOrdinal("branchname")) ? "N/A" : (string)reader["branchname"],
                companyname= reader.IsDBNull(reader.GetOrdinal("companyname")) ? "N/A" : (string)reader["companyname"],
                departmentname= reader.IsDBNull(reader.GetOrdinal("departmentname")) ? "N/A" : (string)reader["departmentname"],
                typename= reader.IsDBNull(reader.GetOrdinal("typename")) ? "N/A" : (string)reader["typename"],
                Users= reader.IsDBNull(reader.GetOrdinal("Users")) ? "N/A" : (string)reader["Users"],
                requested= reader.IsDBNull(reader.GetOrdinal("requested")) ? "N/A" : (string)reader["requested"],
                StatusNow= reader.IsDBNull(reader.GetOrdinal("StatusNow")) ? "N/A" : (string)reader["StatusNow"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"]
            };
         }

        internal async Task Insert(LocationModel location)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_LocationCreate", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add(new SqlParameter("@locid", location.locid));
            //cmd.Parameters.Add(new SqlParameter("@Extradetails", location.Extradetails));
            cmd.Parameters.Add(new SqlParameter("@uid", location.status));
            cmd.Parameters.Add(new SqlParameter("@reqid", location.reqid));
            cmd.Parameters.Add(new SqlParameter("@Asset", location.Asset));
            cmd.Parameters.Add(new SqlParameter("@type", location.type));

            cmd.Parameters.Add(new SqlParameter("@branch", location.branch));
            cmd.Parameters.Add(new SqlParameter("@department", location.department));
            cmd.Parameters.Add(new SqlParameter("@company", location.company));
            cmd.Parameters.Add(new SqlParameter("@floor", location.floor));
            cmd.Parameters.Add(new SqlParameter("@Extradetails", location.Extradetails));
            cmd.Parameters.Add(new SqlParameter("@active", 1));
            cmd.Parameters.Add(new SqlParameter("@Created_at", location.Created_at));
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

        internal async Task<List<LocationModel>> SearchAllLocations_Paginated(string Searchterm, int pageNumber, int pageSize, int lid, int aid, int tid, int uid, int bid, int cid, int did, int rid,int stat)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_SearchAllLocations_Paginated", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Searchterm", Searchterm);
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@lid", lid);
            cmd.Parameters.AddWithValue("@aid", aid);
            cmd.Parameters.AddWithValue("@tid", tid);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@bid", bid);
            cmd.Parameters.AddWithValue("@cid", cid);
            cmd.Parameters.AddWithValue("@did", did);
            cmd.Parameters.AddWithValue("@rid", rid);
           // cmd.Parameters.AddWithValue("@f", f);
            cmd.Parameters.AddWithValue("@stat", stat);
            var response = new List<LocationModel>();
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
