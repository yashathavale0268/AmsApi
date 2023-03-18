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

        internal async Task<List<LocationModel>> GetAllLocations_Paginated(int pageNumber, int pageSize, int lid, int aid, int tid, int uid, int bid, int cid, int did, int rid, int f)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllLocations", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;
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
                cmd.Parameters.AddWithValue("@f", f);
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

        private LocationModel MapToValue(SqlDataReader reader)
        {
            return new LocationModel()
            {
                locid = (int)reader["locid"],
                uid = (int)reader["uid"],
                Asset = (int)reader["Asset"],
                type = (int)reader["type"],
                branch = (int)reader["branch"],
                department = (int)reader["department"],
                company = (int)reader["company"],
                floor = (int)reader["floor"],
                reqid = (int)reader["reqid"],
                Extradetails = (string)reader["Extradetails"],
                active = (bool)reader["active"],
                Assetname = (string)reader["Assetname"],
                branchname=(string)reader["branchname"],
                companyname=(string)reader["companyname"],
                departmentname= (string)reader["departmentname"],
                typename= (string)reader["typename"],
                Users= (string)reader["Users"],
                requested=(string)reader["requested"],
            };
         }

        internal async Task<List<LocationModel>> SearchAllLocations_Paginated(string Searchterm, int pageNumber, int pageSize, int lid, int aid, int tid, int uid, int bid, int cid, int did, int rid, int f)
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
            cmd.Parameters.AddWithValue("@f", f);
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
