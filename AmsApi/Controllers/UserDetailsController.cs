using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AmsApi.Models;
using Microsoft.Extensions.Options;
using AmsApi.Utility;
using AmsApi.Repository;
using System.Data.SqlClient;
using System.Data;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Authorization;

namespace AmsApi.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "Admin,User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly UserDetailsRepository _repository;
        public UserDetailsController(UserDetailsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet("GetAllDetails")]
        public async Task<ActionResult<IEnumerable<UserDetailsModel>>> GetAllDetails([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var msg = new Message();
            var GetDets = await _repository.GetAllDetails(PageNumber, PageSize);
            if (GetDets.Count > 0) { msg.IsSuccess = true; msg.Data = GetDets; }
            else{ msg.ReturnMessage = "novalues fiung";
            }
            return Ok(msg);

        }
        //public async Task<ActionResult<IEnumerable<UserDetailsModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<UserDetailsModel>>> SearchUserDetails([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int searchId = 0, [FromQuery] int depId = 0, [FromQuery] int brcId = 0, [FromQuery] int compId = 0, [FromQuery] int userId = 0, [FromQuery] int floor = 0)
        {
            var msg = new Message();
            var GetDets = await _repository.SearchUserDetails(pageNumber, pageSize, searchTerm,searchId, depId, brcId, compId, userId, floor);
            if (GetDets.Count>0) { 
                msg.IsSuccess = true;
                msg.Data = GetDets;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " no matches found";
            }
            return Ok(msg);
        }
        // GET api/values/5
        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<UserDetailsModel>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetById(id);
               if (response.Count>0) { 
                msg.IsSuccess = true;
                msg.Data = response;
            }
            else
            {
                msg.IsSuccess = false;
                    msg.ReturnMessage = "no values found";
            }
            return Ok(msg);
            
        }

        // POST api/values
        [HttpPost("AddDetails")]
        public async Task<ActionResult> Post([FromBody] UserDetailsModel details)
        {
            var msg = new Message();
            await _repository.Insert(details);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.ItExists = true;
                msg.IsSuccess = false;
                msg.ReturnMessage = "item already registered";
            }
            else if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = " new entry succesfully registered";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "registeration unscessfull";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDetailsModel details)
        {
            var msg = new Message();
            var GetDets = await _repository.GetById(id);
            if (GetDets.Count>0)
            {

            await _repository.UpdateUserDetails(details, id);
            bool success = _repository.IsSuccess;

            if (success is true)
            {
                    msg.IsSuccess = true;
                msg.ReturnMessage = "values updated successfully";
            }
            else
            {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = " update unsuccessfull";
            }
        }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "No  entry found";

            }


            return Ok(msg);

}

// DELETE api/values/5
[HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var msg = new Message();
            var GetDets = await _repository.GetById(id);
            if (GetDets.Count > 0)
            {
                await _repository.DeleteById(id);
                msg.IsSuccess = true;
                msg.ReturnMessage = "succesfully removed";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "removal unsuccessfull";
            }
            return Ok(msg);
        }
    }

    
}


