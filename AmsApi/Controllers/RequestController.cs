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
    [Authorize(Roles = "Admin,User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestRepository _repository;
        public RequestController(RequestRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet("GetAllRequests")]
        
        public async Task<ActionResult<IEnumerable<RequestModel>>> GetAllRequests([FromQuery]int pageNumber=1,[FromQuery] int pageSize=5)
        {
            var msg = new Message();
            var request = await _repository.GetAllRequests(pageNumber,pageSize);
            if(request.Count>0) {
                msg.IsSuccess = true;
                msg.Data = request; } 
            else
            {
                msg.IsSuccess =false;
                msg.ReturnMessage = "no request found";
            }
            return Ok(msg);
        }
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<RequestModel>>> SearchRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] int searchTerm =0,string searchString=null,[FromQuery]int reqId=0,[FromQuery]int assetId=0,[FromQuery]int statId=0)
        {
            var msg = new Message();
            var requests = await _repository.SearchRequests(pageNumber, pageSize, searchTerm,searchString,reqId,assetId,statId);
            if (requests.Count>0){ msg.Data = requests; } else
            {
                msg.ReturnMessage = " no match found";
            }
            return Ok(msg);
        }
        // GET api/values/5
        [HttpGet("Getbyid/{id}")]
        public async Task<ActionResult<RequestModel>> Get(int id = 0)
        {
            var msg = new Message();
            var response = await _repository.GetRequestId(id);
            if (response.Count > 0) 
            {
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
        [HttpPost("CreateNew")]
        public async Task<IActionResult> Post([FromBody] RequestModel request)
        {
            var msg = new Message();
            await _repository.Insert(request);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.IsSuccess = false;
                msg.ItExists = true;
                msg.ReturnMessage = "request alredy registered";
            }
            else if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = " new request succesfully registered";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "request unscessfull";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPut("UpdateRequest/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RequestModel request)
        {
            var msg = new Message();
            var GetRequest = await _repository.GetRequestId(id);
            if (GetRequest.Count>0)
            {
                await _repository.UpdateRequest(request, id);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.ReturnMessage = "request updated successfully";
                }
                else
                {
                    msg.ReturnMessage = "updated unsuccessfull";
                }
            }
            else
            {
                msg.ReturnMessage = "no id found";
            }
           

            return Ok(msg);
        }

        // DELETE api/values/5
        [HttpDelete("DeleteRequest/{id}")]
        public async Task<IActionResult> Delete(int id =0)
        {
            var msg = new Message();
            await _repository.DeleteById(id);
         
            var GetComp = await _repository.GetRequestId(id);

            if (GetComp.Count>0)
            {

                await _repository.DeleteById(id);
                msg.IsSuccess = true;
                msg.ReturnMessage = "Succesfully Deleted";
            }
            else
            {
                msg.ReturnMessage = "no values found";
            }

            return Ok(msg);
        }
    }
}


