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

namespace AmsApi.Controllers
{
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
            var request = await _repository.GetAllRequests(pageNumber,pageSize);
            if(request == null) { return NotFound(); }
            return request;
        }
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<RequestModel>>> SearchRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] int searchTerm =0,string searchString=null,[FromQuery]int reqId=0,[FromQuery]int assetId=0,[FromQuery]int statId=0)
        {
            var requests = await _repository.SearchRequests(pageNumber, pageSize, searchTerm,searchString,reqId,assetId,statId);
            if (requests == null) { return NotFound(); }
            return requests;
        }
        // GET api/values/5
        [HttpGet("Getbyid/{id}")]
        public async Task<ActionResult<RequestModel>> Get(int id)
        {
            var response = await _repository.GetRequestId(id);
            if (response == null) { return NotFound(); }
            return response;
        }

        // POST api/values
        [HttpPost("CreateNew")]
        public async Task Post([FromBody] RequestModel request)
        {
            await _repository.Insert(request);
        }

        // PUT api/values/5
        [HttpPut("UpdateRequest/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RequestModel request)
        {
            var GetRequest = await _repository.GetRequestId(id);
            if (GetRequest == null)
            {
                return NotFound();
            }

            await _repository.UpdateRequest(request, id);

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("DeleteRequest/{id}")]
        public async Task Delete(int id)
        {
            await _repository.DeleteById(id);
        }
    }
}


