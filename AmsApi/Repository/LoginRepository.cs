using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AmsApi.Configuration;
using AmsApi.Models;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AmsApi.Repository
{
    public class LoginRepository
    {
        private readonly string _connectionString;
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
       // private readonly JwtSecurityTokenHandler tokenHandler;
        public bool Itexists { get; private set; }
        public bool IsSuccess { get; private set; }

        public LoginRepository(IConfiguration configuration, IOptions<JwtBearerTokenSettings> jwtTokenOptions)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
            jwtBearerTokenSettings = jwtTokenOptions.Value;
        }
        public async Task<List<UserModel>> GetAll()
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_GetAllUsers", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var response = new List<UserModel>();
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

        public UserModel MapToValue(SqlDataReader reader)
        {
            return new UserModel()
            {
                Userid = (int)reader["UserId"],
                Email = reader["Email"].ToString(),
                Username = reader["Username"].ToString(),
                Role = reader["Role"].ToString(),
                Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                Active = (bool)reader["active"],
            };
        }
        /* dataset approach
    public DataSet MapToDataSet(SqlDataReader reader)
    {
    DataSet dataSet = new DataSet();
    DataTable table = new DataTable();
    dataSet.Tables.Add(table);

    table.Columns.Add("UserId", typeof(int));
    table.Columns.Add("Email", typeof(string));
    table.Columns.Add("Username", typeof(string));
    table.Columns.Add("Active", typeof(bool));

    while (reader.Read())
    {
        DataRow row = table.NewRow();
        row["UserId"] = reader["UserId"];
        row["Email"] = reader["Email"];
        row["Username"] = reader["Username"];
        row["Active"] = reader["Active"];
        table.Rows.Add(row);
    }

    return dataSet;
    }
         */
        public async Task<UserModel> GetById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_GetAllUsers", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    UserModel response = null;
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

       


        // Hash the password using SHA2 --SHA512(Out dated now)
        public static string HashPassword(string password)
        {
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        internal Task GetUserRole(UserModel user)
        {
            throw new NotImplementedException();
        }

        public async Task Insert(UserModel user)
        {
           
            // string hashedPassword = Argon2.Hash(password);
            string password = user.Password;
            string hashedpassword = HashPassword(password);
            using (SqlConnection sql = new(_connectionString))
            {
                await sql.OpenAsync();
                using (SqlCommand cmd = new("sp_register_user", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Username", user.Username));
                    cmd.Parameters.Add(new SqlParameter("@Email", user.Email));
                    cmd.Parameters.Add(new SqlParameter("@Password", hashedpassword));
                    cmd.Parameters.Add(new SqlParameter("@active", 1));

                    // returncode = new SqlParameter("@ReturnCode", SqlDbType.NVarChar) { Direction = ParameterDirection.Output };
                    //cmd.Parameters.Add(returncode);
                    var returncode = new SqlParameter("@exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returncode);
                    var returnnote = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(returnnote);


                    
                    await cmd.ExecuteNonQueryAsync();
                  await  sql.CloseAsync();
                    
                    bool itexists = returncode?.Value is not DBNull && (bool)returncode.Value;
                    bool successfull = returnnote?.Value is not DBNull && (bool)returnnote.Value;
                   
                    Itexists = itexists;
                    IsSuccess = successfull;

                    return;
                }
            }
        }

        internal async Task<UserModel> GetAllUser(int pageNumber, int pageSize)
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAllUsers", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            UserModel response = new();
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

        public async Task<UserModel> SearchUsers(int pageNumber, int pageSize, string searchTerm, int User)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using SqlCommand cmd = new("sp_SearchAllUser_Paginated", sql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                cmd.Parameters.AddWithValue("@User", User);


                UserModel response = new();
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response= MapToValue(reader);
                    }
                }

                return response;
            }
        }
        //public async Task Verify(UserMaster user)
        //{
        //    using (SqlConnection sql = new SqlConnection(_connectionString))
        //    {
        //        await sql.OpenAsync();

        //        using (var command = new SqlCommand("login_user", sql))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@UserName", user.Email);
        //            command.Parameters.AddWithValue("@Password", user.Password);

        //            var isAuthenticatedParam = new SqlParameter("@isAuthenticated", SqlDbType.Bit) { Direction = ParameterDirection.Output };
        //            command.Parameters.Add(isAuthenticatedParam);
        //            var isAdmin = new SqlParameter("@isAdmin", SqlDbType.Bit) { Direction = ParameterDirection.Output };
        //            command.Parameters.Add(isAdmin);
        //            await sql.OpenAsync();
        //            await command.ExecuteNonQueryAsync();
        //            sql.Close();
        //            isAuth = (bool)isAuthenticatedParam.Value;
        //            Admin = (bool)isAdmin.Value;

        //        }
        public async Task <UserModel> GetbyObj(UserModel user) //LIST<>
        {
            string password = user.Password;
            //,bool Admin
            string hashedpassword = HashPassword(password);
            using (SqlConnection sql = new(_connectionString))
            {
                await sql.OpenAsync();

                using (SqlCommand command = new("sp_login_user", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", user.Email);
                    command.Parameters.AddWithValue("@Password", hashedpassword);

                   

                    
                    await command.ExecuteNonQueryAsync();


                 

                    UserModel response = new();
                   // var response = new List<AssetModel>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response =getuserbyname(reader);
                        }
                    }
                    return response ;


                }

            }

        }
        private UserModel getuserbyname(SqlDataReader reader)

        {
            return new UserModel()
            {
                Userid = (int)reader["UserId"],
                Email = reader["Email"].ToString(),
                Username = reader["Username"].ToString(),
                Role = reader["Role"].ToString(),
               
            };
        }
    
        //-----------Role Update and set
        public async Task SetRoles(String Role,int id)
        { 
            using (SqlConnection sql = new(_connectionString))
            {
                await sql.OpenAsync();
                using (SqlCommand command = new("sp_SetRole", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    
                    command.Parameters.AddWithValue("@Role", Role);

                    await command.ExecuteNonQueryAsync();
                }
            }
            return;
        }

        public async Task DeleteById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_DeleteUsers", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }




        #region update for users
        internal async Task<UserModel> GetUserById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                await sql.OpenAsync();
                using (SqlCommand command = new("sp_SearchUsers", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);

                    var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        return new UserModel
                        {
                            Userid = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3),
                            Role = reader.GetString(4),



                        };
                    }

                    return null;
                }
            }
        }

        internal async Task UpdateUser(UserModel user, int id)
        {
            //string hashedPassword = BCrypt.HashPassword(password);
            string password = user.Password;
            string hashedpassword = HashPassword(password);
            using (SqlConnection sql = new(_connectionString))
            {
                await sql.OpenAsync();
                using (SqlCommand command = new("sp_UpdateUsers", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", hashedpassword);

                    var returncode = new SqlParameter("@exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(returncode);
                    var returnnote = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(returnnote);
                    await command.ExecuteNonQueryAsync();

                    bool itexists = returncode?.Value is not DBNull && (bool)returncode.Value;
                    bool successfull = returnnote?.Value is not DBNull && (bool)returnnote.Value;

                    Itexists = itexists;
                    IsSuccess = successfull;


                    return;
                }
            }
           
        }
        internal object GenerateToken(UserModel userSessions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userSessions.Username.ToString()),
                    new Claim(ClaimTypes.Email, userSessions.Email.ToString()),
                    new Claim(ClaimTypes.Role,userSessions.Role.ToString()),

                }),

                Expires = DateTime.Now.AddMinutes(jwtBearerTokenSettings.ExpiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.ValidAudience,
                Issuer = jwtBearerTokenSettings.ValidIssuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
           
            return tokenHandler.WriteToken(token);

        }

        //internal string DecodeJwtPayload(string tokenval)
        //{
        //    //JwtBearerTokenSettings tokenstats = new();
        //    string encodedPayload = tokenval.Split('.')[1];
        //    //string key = Encoding.UTF8.GetBytes(tokenstats.SecretKey);
        //    string decodedPayload = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPayload));
        //  //  var valueBytes = System.Convert.FromBase64String(tokenval);
        //    return decodedPayload;
        //}

        //internal JwtPayLoad DecodeJwtPayload(string tokenval)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(tokenval);

        //    var payload = jwtToken.Payload.SerializeToJson();
        //    var jwtPayLoad = JsonConvert.DeserializeObject<JwtPayLoad>(payload);
        //    return payload;
        //}
        public JwtPayLoad DecodeJwtPayload(string token)
        {
            var parts = token.Split('.');
            var payload = parts[1];
            var payloadBytes = Convert.FromBase64String(Pad(payload));
            var jsonPayload = Encoding.UTF8.GetString(payloadBytes);
            var jwtPayload = JsonConvert.DeserializeObject<JwtPayLoad>(jsonPayload);
            return jwtPayload;

        }

        // helper function to pad the base64 string
        private static string Pad(string base64)
        {
            switch (base64.Length % 4)
            {
                case 0:
                    return base64;
                case 2:
                    return base64 + "==";
                case 3:
                    return base64 + "=";
                default:
                    throw new Exception("Illegal base64url string!");
            }
        }

        // JwtPayload class
        
        #endregion
    }
}

