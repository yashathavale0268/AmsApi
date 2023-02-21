using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AmsApi.Models;
using AmsApi.Repository;
using CoreApiAdoDemo.Model;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ={}]
  //[Authorize]
    public class RegisterationController : ControllerBase
    {

        private readonly LoginRepository _repository;
        public RegisterationController(LoginRepository repository)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));

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


        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<IEnumerable<UserModel>>> Userlogin([FromBody] UserModel user)
        {
            
            var msg = new Message<UserModel>();
            // ,bool Admin//,Admin

            UserModel userSessions = new();
            userSessions =  await _repository.GetbyObj(user);
           

            if (userSessions.Userid>0)
            { 
                msg.IsSuccess = true;
                msg.ReturnMessage = "Successful Login";

                if (userSessions.Userid>0)
                {
                    msg.ReturnMessage = "Successful Login";

                    string token = (string)_repository.GenerateToken(userSessions);
                    JwtPayLoad tokenvalues = new();
                    //var tokendecode = HMACSHA256(base64UrlEncode(header) + "." + base64UrlEncode(payload),your - 256 - bit - secret)

                    tokenvalues = _repository.DecodeJwtPayload(token); //,key

                    //return Ok(new { Token = tokenvalues, Message = "Success" });//tokenvalues
                    return Ok(tokenvalues);

                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = " no user found";
                    throw new ArgumentNullException(nameof(userSessions));



                }

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
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUser([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var msg = new Message<UserModel>();
            var Users = await _repository.GetAllUser(PageNumber, PageSize);
            if (Users.Count>0) {
                
                return Users;
            }
            else
            {
               
                msg.ReturnMessage = "no user found";
            }
            return Ok(msg);
        }
        //public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<UserModel>>> SearchUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null,[FromQuery] int User =0)
        {
            var msg = new Message<UserModel>();
            var users = await _repository.SearchUsers(pageNumber,pageSize,searchTerm,User);
            if (users.Count>0) { return users; }
            else { msg.ReturnMessage = "no match found"; }
            return NotFound(msg);
        }
        [HttpGet("Searchbyid/{id}")]
        
        public async Task<ActionResult<UserModel>> Get(int id=0)
        {
            var msg = new Message<UserModel>();
            var response = await _repository.GetById(id);
            if(response.Userid>0) { return response;}
            else
            {
                msg.ReturnMessage = "no id found";
                return NotFound(msg);
            }
           
        }
        [HttpPost]
        [Route("NewUser")]
        public async Task<ActionResult<UserModel>> Post([FromBody] UserModel user)
        {
            var msg = new Message<UserModel>();
            await _repository.Insert(user);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;
            if (exists is true)
            {
             
                msg.ReturnMessage = " Same Credential Already exists try  with another Credentials ";
            }
            else if (success is true)
            {
                
                msg.ReturnMessage = " New  Registeration  is Success";
                
            }
            else
            {
                msg.ReturnMessage = " Registeration is unsuccessful";
            }

            return Ok(msg);

        }
        [HttpPost("SetRole/{id}")]
        public async Task<IActionResult> SetRole([FromQuery] string Role = "N/A",int id = 0 )
        {
            var msg = new Message<UserModel>();
            var User = await _repository.GetUserById(id);
            if (User == null)
            {
                return NotFound();
            }
            else if (User is not null)
            {
                await _repository.SetRoles(Role,id);
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
        public async Task<IActionResult> Update( [FromBody] UserModel user,int id= 0)
        {
            var msg = new Message<UserModel>();
            var User = await _repository.GetUserById(id);
            if (User.Userid>0)
            {
                await _repository.UpdateUser(user,id);

                if (msg.IsSuccess is true)
                {

                  
                    msg.ReturnMessage = " User is Updated Successfully";
                }
                else
                {
                    msg.ReturnMessage = " User is Update is unsuccessfull";
                }
            }
            else
            {
                msg.ReturnMessage = " no user found";
            }
            return Ok(msg);
        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var msg = new Message<AssettypeModel>();
            var users = await _repository.GetUserById(id);

            if (users.Userid > 0)
            {

                await _repository.DeleteById(id);
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
    
}

    
}
