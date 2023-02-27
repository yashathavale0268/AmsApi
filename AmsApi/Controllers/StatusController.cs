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
    [Authorize]
    
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusRepository _repository;
        public StatusController(StatusRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllStatus")]
        public async Task<ActionResult<IEnumerable<StatusModel>>> GetAllStatus([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var msg = new Message();
            var status = await _repository.GetAllStatus(PageNumber, PageSize);
            if (status.Count>0) { msg.Data=status; }
            return status;
        }
        //public async Task<ActionResult<IEnumerable<StatusModel>>> Get()
        //{
        //    return await _repository.GetAllStatus();
        //}

        // GET api/values/5
        [Authorize(Roles = "Admin,User")]
        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<StatusModel>> Get(int id)
        {
            var msg = new Message();
            var response = 
                await _repository.GetStatusById(id);
         //  if (response == null) { return NotFound(); }
         //   return response;
            return Ok();
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<StatusModel>>> SearchStatus([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int Userid = 0, [FromQuery] int Assetid = 0, [FromQuery] int Requestid = 0,[FromQuery] int Statid = 0)
        {
            var msg = new Message();
            var requests = await _repository.SearchStatus(pageNumber,pageSize, searchTerm,Userid,Assetid,Requestid,Statid);
            if (requests == null) { return NotFound(); }
            return requests;
        }
        // POST api/values
        [HttpPost("NewStatus")]
        public async Task NewStatus([FromBody] StatusModel company)
        {
            await _repository.Insert(company);
        }

        // PUT api/values/5
        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update( [FromBody] StatusModel stat, int id=0)
        {
            var msg = new Message();
            var GetStatus = await _repository.GetStatusById(id);
            if (GetStatus.Count>0)
            {
                await _repository.UpdateStatus(stat, id);
                bool success = _repository.IsSuccess;

                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "values updated successfully";
                }
                else
                {
                    msg.ReturnMessage = " update unsuccessfull";
                }
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no id found";
            }

           

            return Ok(msg);
        }

        // DELETE api/values/5
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var msg = new Message();
            var GetStatus = await _repository.GetStatusById(id);
            if (GetStatus.Count > 0)
            {
                await _repository.DeleteById(id);
                msg.ReturnMessage = "succesfully removed";
            }
            else
            {
                msg.ReturnMessage = "removal unsuccessfull";
            }
            return Ok(msg);
}
    }
}


