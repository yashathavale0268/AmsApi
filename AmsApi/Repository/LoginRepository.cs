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
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Newtonsoft.Json;

namespace AmsApi.Repository
{
    public class LoginRepository
    {
        private readonly string _connectionString;
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
       // private readonly JwtSecurityTokenHandler tokenHandler;
        public bool Itexists { get; set; }
        public bool IsSuccess { get;  set; }
        Message msg = new();
        public LoginRepository(IConfiguration configuration, IOptions<JwtBearerTokenSettings> jwtTokenOptions)
        {
            _connectionString = configuration.GetConnectionString("MainCon");
            jwtBearerTokenSettings = jwtTokenOptions.Value;
        }

        public DataSet GetAllTables()
        {
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new("sp_GetAll", sql);
            {

                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }
        //public async Task<List<UserModel>> GetAllUserstable()
        //{
        //    using (SqlConnection sql = new(_connectionString))
        //    {
        //        using (SqlCommand cmd = new("sp_GetAllUsers", sql))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            var response = new List<UserModel>();
        //           // UserModel response = null;
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

        public UserModel MapToValue(SqlDataReader reader)
        {
            return new UserModel()
            {
                Userid = (int)reader["UserId"],
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "N/A" : (string)reader["Email"],
                Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? "N/A" : (string)reader["Username"],
                First_name = reader.IsDBNull(reader.GetOrdinal("First_name")) ? "N/A" : (string)reader["First_name"],
                Last_name = reader.IsDBNull(reader.GetOrdinal("Last_name")) ? "N/A" : (string)reader["Last_name"],
                Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? 0: (int)reader["Department"],
                Branch = reader.IsDBNull(reader.GetOrdinal("Branch")) ? 0: (int)reader["Branch"],
                Floor = reader.IsDBNull(reader.GetOrdinal("Floor")) ? 0: (int)reader["Floor"],
                Company = reader.IsDBNull(reader.GetOrdinal("Company")) ? 0: (int)reader["Company"],
                Role = reader.IsDBNull(reader.GetOrdinal("Role")) ? 0: (int)reader["Role"],
                RoleName= reader.IsDBNull(reader.GetOrdinal("RoleName")) ? "N/A" : (string)reader["RoleName"],
                //Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                //active = (bool)reader["active"],
                DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? "N/A" : (string)reader["DepartmentName"],
                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? "N/A" : (string)reader["CompanyName"],
                BranchName = reader.IsDBNull(reader.GetOrdinal("BranchName")) ? "N/A" : (string)reader["BranchName"],
                Full_name = reader.IsDBNull(reader.GetOrdinal("Full_name")) ? "N/A" : (string)reader["Full_name"],
                totalrecord = reader.IsDBNull(reader.GetOrdinal("totalrecord")) ? 0 : (int)reader["totalrecord"]
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
        public async Task<UserModel> GetById(UserModel user)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                using (SqlCommand cmd = new("sp_SearchAllUser_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User",user.Userid));
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

        internal string GeneratePasswordResetToken()
        {
            var token = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(token);
                return Convert.ToBase64String(token);
            }
        }
        internal async Task SendPasswordResetEmail(string email, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Erik Reynolds", "erik.reynolds@ethereal.email"));
            message.To.Add(new MailboxAddress("User",email));
            message.Subject = "Password Reset";
            message.Body = new TextPart("plain")
            {
                Text = $"To reset your password, please click the following link:https://localhost:44377/api/Registeration/password-reset-confirm?email={email}&token={token}"
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("erik.reynolds@ethereal.email", "awDq63ZJVr9p9trG8p");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
//         using (var command = new SqlCommand("UPDATE Users SET PasswordResetToken = @Token WHERE Email = @Email", connection))
//                {
//                    command.Parameters.AddWithValue("@Token", token);
//                    command.Parameters.AddWithValue("@Email", model.Email);
//                    var result = await command.ExecuteNonQueryAsync();
//                    if (result != 1)
//                    {
//                        return BadRequest("Failed to generate reset token");
//}
//                }

                                
//            }

//            return Ok();
//        }
        //public object Validatetoken(string token, string tokenkey)
        //{
        //    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey));
        //    //var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);
        //    var tokenHandler = new JwtSecurityTokenHandler();


        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = key,
        //            ValidateIssuer = false,
        //            ValidateAudience = false
        //        }, out SecurityToken validatedToken);
        //    string SecurityToken = tokenHandler.WriteToken(validatedToken);
        //    return SecurityToken;
        //}

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
                  
                    using (SqlCommand cmd = new("sp_register_user", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", hashedpassword);
                        cmd.Parameters.AddWithValue("@First_name", user.First_name);
                        cmd.Parameters.AddWithValue("@Last_name", user.Last_name);
                        cmd.Parameters.AddWithValue("@Dep", user.Department);
                        cmd.Parameters.AddWithValue("@Branch", user.Branch);
                        cmd.Parameters.AddWithValue("@Floor", user.Floor);
                        cmd.Parameters.AddWithValue("@Comp", user.Company);
                        cmd.Parameters.AddWithValue("@active", 1);
                        await sql.OpenAsync();
                        // returncode = new SqlParameter("@ReturnCode", SqlDbType.NVarChar) { Direction = ParameterDirection.Output };
                        //cmd.Parameters.Add(returncode);
                        var returncode = new SqlParameter("@exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(returncode);
                        var returnnote = new SqlParameter("@success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(returnnote);



                        await cmd.ExecuteNonQueryAsync();
                       

                        bool itexists = returncode?.Value is not DBNull && (bool)returncode.Value;
                        bool successfull = returnnote?.Value is not DBNull && (bool)returnnote.Value;

                        Itexists = itexists;
                        IsSuccess = successfull;

                        return;
                    }
                }
        }
            
        

        //internal async Task<List<UserModel>> GetAllUser(int pageNumber, int pageSize)
        //{
        //    using SqlConnection sql = new(_connectionString);
        //    using SqlCommand cmd = new("sp_GetAllUsers", sql);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        //    cmd.Parameters.AddWithValue("@PageSize", pageSize);
        //    //   UserModel response = new();
        //    var response = new List<UserModel>();
        //   await sql.OpenAsync();

        //    using (var reader = await cmd.ExecuteReaderAsync())
        //    {
        //        while (await reader.ReadAsync())
        //        {
        //            response.Add(MapToValue(reader));
        //        }
        //    }

        //    return response;
        //}

        public async Task<List<UserModel>> SearchUsers(int pageNumber, int pageSize, string searchTerm, int User)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using SqlCommand cmd = new("sp_SearchAllUser_Paginated", sql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                cmd.Parameters.AddWithValue("@User", User);

                var response = new List<UserModel>();
               // UserModel response = new();
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

        

        internal object GenerateemailToken(UserModel email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Userid", email.Userid.ToString()),
                    new Claim(ClaimTypes.Name, email.Username.ToString()),
                    new Claim(ClaimTypes.Email, email.Email.ToString()),
                    new Claim(ClaimTypes.Role,email.Role.ToString()),
               
                }),

                Expires = DateTime.Now.AddMinutes(jwtBearerTokenSettings.ExpiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.ValidAudience,
                Issuer = jwtBearerTokenSettings.ValidIssuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        internal Task PasswordRecoveryToken(string email, string token)
        {
            throw new NotImplementedException();
        }

        internal async Task<UserModel> GetByemail(PasswordReset email)
        {
            using (SqlConnection sql = new(_connectionString))
            using (SqlCommand cmd = new("sp_checkemail", sql))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email.Email);

                var returncode = new SqlParameter("@exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(returncode);
                await sql.OpenAsync();
                UserModel response = new();
                // var response = new List<AssetModel>();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response = getnamebyemail(reader);
                    }
                }
                await sql.CloseAsync();

                bool itexists = returncode?.Value is not DBNull && (bool)returncode.Value;
                
                Itexists = itexists;


                return response;
            }
            
        }

        internal async Task ChangePassword(UserModel user)
        {
            string password = user.Password;
            string hashedpassword = HashPassword(password);
            using (SqlConnection sql = new(_connectionString))
            {
                await sql.OpenAsync();
                using (SqlCommand command = new("sp_ChangePassword", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", user.Userid);
                    command.Parameters.AddWithValue("@Password", hashedpassword);
                    var returncode = new SqlParameter("@exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(returncode);
                    var returnnote = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
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

        internal async Task GetIDForCheck(UserModel user)
        {
            using (SqlConnection sql = new(_connectionString))
            {
                await sql.OpenAsync();
                using (SqlCommand command = new("sp_GetPassword", sql))
                {
                    string password = user.Password;
                    string hashedpassword = HashPassword(password);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", user.Userid);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", hashedpassword);
                    //command.Parameters.AddWithValue("@Password", user.Password);
                    //command.Parameters.AddWithValue("@Password", hashedpassword);
                    var returncode = new SqlParameter("@exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(returncode);
                    //var returnnote = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    //command.Parameters.Add(returnnote);
                    await command.ExecuteNonQueryAsync();

                    bool itexists = returncode?.Value is not DBNull && (bool)returncode.Value;
                  //  bool successfull = returnnote?.Value is not DBNull && (bool)returnnote.Value;

                    Itexists = itexists;
                    //IsSuccess = successfull;


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
                    new Claim("Userid", userSessions.Userid.ToString()),
                    new Claim(ClaimTypes.Name, userSessions.Username.ToString()),
                    new Claim(ClaimTypes.Email, userSessions.Email.ToString()),
                    new Claim(ClaimTypes.Role,userSessions.Role.ToString()),
                    new Claim("Department",userSessions.Department.ToString()),
                    new Claim("Branch",userSessions.Branch.ToString()),
                    new Claim("Company",userSessions.Company.ToString()),
                    new Claim("Floor",userSessions.Floor.ToString()),
                    new Claim("Full_name",userSessions.Full_name.ToString()),
                }),

                Expires = DateTime.Now.AddMinutes(jwtBearerTokenSettings.ExpiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.ValidAudience,
                Issuer = jwtBearerTokenSettings.ValidIssuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
        private UserModel getuserbyname(SqlDataReader reader)

        {
            return new UserModel()
            {
               
                Userid = (int)reader["UserId"],
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "N/A" : (string)reader["Email"],
                Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? "N/A" : (string)reader["Username"],
                First_name = reader.IsDBNull(reader.GetOrdinal("First_name")) ? "N/A" : (string)reader["First_name"],
                Last_name = reader.IsDBNull(reader.GetOrdinal("Last_name")) ? "N/A" : (string)reader["Last_name"],
                Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? 0 : (int)reader["Department"],
                Branch = reader.IsDBNull(reader.GetOrdinal("Branch")) ? 0 : (int)reader["Branch"],
                Floor = reader.IsDBNull(reader.GetOrdinal("Floor")) ? 0 : (int)reader["Floor"],
                Company = reader.IsDBNull(reader.GetOrdinal("Company")) ? 0 : (int)reader["Company"],
                Role = reader.IsDBNull(reader.GetOrdinal("Role")) ? 0 : (int)reader["Role"],
                RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? "N/A" : (string)reader["RoleName"],
                //Created_at = (reader["Created_at"] != DBNull.Value) ? Convert.ToDateTime(reader["Created_at"]) : DateTime.MinValue,
                //active = (bool)reader["active"],
                DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? "N/A" : (string)reader["DepartmentName"],
                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? "N/A" : (string)reader["CompanyName"],
                BranchName = reader.IsDBNull(reader.GetOrdinal("BranchName")) ? "N/A" : (string)reader["BranchName"],
                Full_name = reader.IsDBNull(reader.GetOrdinal("Full_name")) ? "N/A" : (string)reader["Full_name"],
               
            };
        }
        private UserModel getnamebyemail(SqlDataReader reader)

        {
            return new UserModel()
            {
                Userid = (int)reader["UserId"],
                Email = reader["Email"].ToString(),
                Username = reader["Username"].ToString(),
                Role= (int)reader["Role"],
                 RoleName = (string)reader["RoleName"],
            };
        }

        //-----------Role Update and set
        public async Task SetRoles(int Role,int id)
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




      
        internal async Task<List<UserModel>> GetUserById(int id)
        {
            using (SqlConnection sql = new(_connectionString))
            {

                using (SqlCommand cmd = new("sp_SearchAllUser_Paginated", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User", id);

                    var response = new List<UserModel>();
                    // UserModel response = new();
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

        internal async Task UpdateUser(UserModel user)
        {
            //string hashedPassword = BCrypt.HashPassword(password);
           // string password = user.Password;
           // string hashedpassword = HashPassword(password);
            using (SqlConnection sql = new(_connectionString))
            {
                await sql.OpenAsync();
                using (SqlCommand command = new("sp_UpdateUsers", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id",user.Userid);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    //command.Parameters.AddWithValue("@Password", user.Password);
                    //command.Parameters.AddWithValue("@Password", hashedpassword);
                    command.Parameters.AddWithValue("@First_name", user.First_name);
                    command.Parameters.AddWithValue("@Last_name", user.Last_name);
                    command.Parameters.AddWithValue("@Dep", user.Department);
                    command.Parameters.AddWithValue("@Branch", user.Branch);
                    command.Parameters.AddWithValue("@Floor", user.Floor);
                    command.Parameters.AddWithValue("@Comp", user.Company);
                    //var returncode = new SqlParameter("@exists", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    //command.Parameters.Add(returncode);
                    var returnnote = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(returnnote);
                    await command.ExecuteNonQueryAsync();

                    //bool itexists = returncode?.Value is not DBNull && (bool)returncode.Value;
                    bool successfull = returnnote?.Value is not DBNull && (bool)returnnote.Value;

                    //Itexists = itexists;
                    IsSuccess = successfull;


                    return;
                }
            }
           
        }


        #region decode the token ?
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

        //------------------------------------------------------------------------------
        // working decoder
        //---------------------------------------------------------------------------
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

