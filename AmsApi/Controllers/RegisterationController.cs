using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AmsApi.Configuration;
using AmsApi.Models;
using AmsApi.Repository;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AmsApi.Controllers
{
    [AllowAnonymous]
    //[Authorize]

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ={}]
    //[Authorize]
    public class RegisterationController : ControllerBase
    {

        private readonly LoginRepository _repository;
        private readonly AmsRepository _common;
        //  private readonly string key;
        public RegisterationController(LoginRepository repository, AmsRepository common, ILogger<RegisterationController> logger)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));

            this._common = common ?? throw new ArgumentNullException(nameof(common));
        }

        #region login old
        //public string Login(UserMaster login)

        //{
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MainCon").ToString());
        //        SqlDataAdapter Dataset = new SqlDataAdapter("login_user", con);
        //        //SELECT * FROM Registeration where Email='" + registeration.Email + "'AND Password ='" + registeration.Password +"' AND IsActive = 1 
        //        DataTable Data = new DataTable();
        //        con.Open();

        //        using (var command = new SqlCommand("authenticate_user", con))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@UserName", login.Email);
        //            command.Parameters.AddWithValue("@Password", login.Password);

        //            var isAuthenticatedParam = new SqlParameter("@isAuthenticated", SqlDbType.Bit) { Direction = ParameterDirection.Output };
        //            command.Parameters.Add(isAuthenticatedParam);
        //            var isAdmin = new SqlParameter("@isAdmin", SqlDbType.Bit) { Direction = ParameterDirection.Output };
        //            command.Parameters.Add(isAdmin);

        //            command.ExecuteNonQuery();
        //            con.Close();
        //            var isAuthenticated = (bool)isAuthenticatedParam.Value;
        //            var Admin = (bool)isAdmin.Value;
        //            if (isAuthenticated == true && Admin == true )
        //            {
        //                // generate a JWT token and return it to the client

        //               return Ok().ToString();
        //               // return RedirectToAction(AdminPan);
        //            }
        //            else
        //            {
        //                return Unauthorized().ToString();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return e.ToString();
        //    }
        //}
        #endregion
      
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UserModel>> Userlogin([FromBody] UserModel user)
        {

            var msg = new Message();
            // ,bool Admin//,Admin
            //throw new ArgumentNullException(nameof(userSessions));
            ////,key //var tokendecode = HMACSHA256(base64UrlEncode(header) + "." + base64UrlEncode(payload),your - 256 - bit - secret) //return Ok(new { Token = tokenvalues, Message = "Success" });//tokenvalues
          //  UserModel userSessions = new();
           // JwtPayLoad tokenvalues = new();
           var userSessions = await _repository.GetbyObj(user);


            if (userSessions.Userid > 0)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = "Successful Login";


                var token = _repository.GenerateToken(userSessions); //null;//
                                                                     //string secretKey = key;
                                                                     //string tokenkey = secretKey;
                                                                     //msg.Data =tokenkey;
                msg.Data = (token, userSessions);




                //var validatedtoken = _repository.Validatetoken(token, tokenkey);
                //msg.Data = validatedtoken;

                ////---if want to decode the token later---
                //  tokenvalues = _repository.DecodeJwtPayload((token).ToString());
                //   msg.Data = tokenvalues;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " no user found";
                //throw new ArgumentNullException(nameof(userSessions));

            }



            return Ok(msg);
        }

        #region claims
        // HttpContext.User = _repository.;
        //return Ok("Successful login");


        //var userId = HttpContext.user.FindFirstValue("Userid");
        //var user = await _repository.user.SingleOrDefaultAsync(x => x.Id == userid);
        //var role = user.Role;
        // var role = await _repository.GetUserRole(user);/*user, email and role info */

        //var claimsIdentity = new ClaimsIdentity(new[]
        //{

        //            new Claim(ClaimTypes.Name, user.Username),
        //            new Claim(ClaimTypes.Email, user.Email),
        //            new Claim(ClaimTypes.Role, user.Role),
        //            }, "Token");
        //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


        //var claimsIdentity = new ClaimsIdentity();
        //claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        //HttpContext.User = new ClaimsPrincipal(claimsIdentity);
        #endregion

        // [Authorize(Policy = "Adminonly")]
        /*  public async Task<ActionResult<UserModel>> GetAll()  //[FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var Users = await _repository.GetAll()
            if (Users == null) {
               
                msg.ReturnMessage = "no user found";

            }
            else if(Users.Userid>0)
            {
                msg.Data = Users;
                msg.IsSuccess = true;
              

            }
            return Ok(msg);*/
        // [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)  //
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var Users = await _repository.GetAllUser(PageNumber, PageSize);
            if (Users.Count > 0)
            {

                msg.IsSuccess = true;
                msg.Data = Users;

            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no user found";
            }
            return Ok(msg);
        }
        //public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        //{
        //    return await _repository.GetAll();
        //}
        //[Authorize(Roles = "Admin")]
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<UserModel>>> SearchUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int User = 0)
        {
            var msg = new Message();
            var Users = await _repository.SearchUsers(pageNumber, pageSize, searchTerm, User);
            if (Users.Count > 0)
            {

                msg.IsSuccess = true;
                msg.Data = Users;

            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no match found";
            }
            return Ok(msg);
        }
        // [Authorize(Roles = "Admin")]
        [HttpGet("Searchbyid/{id}")]

        public async Task<ActionResult<UserModel>> Get(int id = 0)
        {
            var msg = new Message();
            var response = await _repository.GetById(id);
            if (response.Userid > 0)
            {
                msg.IsSuccess = true;
                msg.Data = response;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no id found";

            }
            return Ok(msg);
        }
        //}
        [AllowAnonymous]
        [HttpGet("GetAllTables")]
        public IActionResult GetAllTables()
        {
            var msg = new Message();
            var result = _repository.GetAllTables();
           
           if(result.Tables.Count>0)
            {
                msg.IsSuccess = true;
                msg.Data = result;
               
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values available";

            }
        
           
            return Ok(msg);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("NewUser")]
        public async Task<ActionResult<UserModel>> Post([FromBody] UserModel user)
        {
            
            var msg = new Message();
           
            
            await _repository.Insert(user);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " Same Credential Already exists try  with another Credentials ";
            }
            else if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = " New  Registeration  is Success";

            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " Registeration is unsuccessful";
            }

            return Ok(msg);

        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("SetRole/{id}")]
        public async Task<IActionResult> SetRole([FromQuery] string Role = "N/A", int id = 0)
        {
            var msg = new Message();
            var User = await _repository.GetUserById(id);
            if (User == null)
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no user found";
            }
            else if (User is not null)
            {
                await _repository.SetRoles(Role, id);
                msg.IsSuccess = true;
                msg.ReturnMessage = " User is Updated Successfully";
            }
            return Ok(msg);
        }

        //[HttpPost]
        //public async Task Verify([FromBody] UserMaster user)
        //{
        //    await _repository.Verify(user);
        //}
        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] UserModel user, int id = 0)
        {
           
            var msg = new Message();
            var User = await _repository.GetById(id);
           
             if (User.Userid > 0)
            {
               // msg.Data = User;

                await _repository.UpdateUser(user, id);
                bool success = _repository.IsSuccess;
                if (success is true)
                {

                    msg.IsSuccess = true;
                    msg.ReturnMessage = " User is Updated Successfully";
                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = " User update unsuccessfull";
                }
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " no user found";

            }
            return Ok(msg);
        }


        [HttpPut("UpdatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword([FromBody] UserModel user, int id = 0)
        {
            var msg = new Message();
            await _repository.GetIDForCheck(user, id);
            
                bool itexists = _repository.Itexists;
                if (itexists is true)
                {
                     await _repository.ChangePassword(user, id);
                bool exists = _repository.Itexists;   
                bool success = _repository.IsSuccess;
                            if (exists is true)
                            {
                                msg.IsSuccess = false;
                                msg.ReturnMessage = "please  enter a different password";
                            }
                                        else if (success is true)
                                        {

                                            msg.IsSuccess = true;
                                            msg.ReturnMessage = " password is Updated Successfully";
                                        }
                             else
                             {
                                 msg.IsSuccess = false;
                                 msg.ReturnMessage = " password update unsuccessfull";
                             }
                            }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "  no valid credentials found try again ";
                }
                return Ok(msg);
            
        }

        // DELETE api/values/5
       // [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var msg = new Message();
            var Users = await _repository.GetUserById(id);

            if (Users.Userid> 0)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = "Delete Successfully";

                await _repository.DeleteById(id);
                msg.IsSuccess = true;
                msg.ReturnMessage = " values deleted";
            }
            else
            {
                msg.ReturnMessage = "no values found";
            }
          return Ok(msg);
        }

        //[HttpPost]
        //[Route("Logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    //logout after time experies
        //    return Ok(new { Token = "", Message = "Logged Out" });
        //}


        //[HttpGet]
        //[Route("api/names")]
        //public async Task<ActionResult<IEnumerable<Commondbo>>> Giveall()
        //{
        //    Commondbo commondbo = new();
        //    commondbo = await _common.giveAll();
            
        //}

    }


}
