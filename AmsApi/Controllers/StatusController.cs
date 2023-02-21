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
    public class StatusController : ControllerBase
    {
        private readonly StatusRepository _repository;
        public StatusController(StatusRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusModel>>> GetAllStatus([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var status = await _repository.GetAllStatus(PageNumber, PageSize);
            if (status == null) { return NotFound(); }
            return status;
        }
        //public async Task<ActionResult<IEnumerable<StatusModel>>> Get()
        //{
        //    return await _repository.GetAllStatus();
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusModel>> Get(int id)
        {
         // var response = 
                await _repository.GetStatusById(id);
         //  if (response == null) { return NotFound(); }
         //   return response;
            return Ok();
        }
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<StatusModel>>> SearchStatus([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int Userid = 0, [FromQuery] int Assetid = 0, [FromQuery] int Requestid = 0,[FromQuery] int Statid = 0)
        {
            var requests = await _repository.SearchStatus(pageNumber,pageSize, searchTerm,Userid,Assetid,Requestid,Statid);
            if (requests == null) { return NotFound(); }
            return requests;
        }
        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] StatusModel company)
        {
            await _repository.Insert(company);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StatusModel stat)
        {
            var GetStatus = await _repository.GetStatusById(id);
            if (GetStatus == null)
            {
                return NotFound();
            }

            await _repository.UpdateStatus(stat, id);

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task Delete(int id)
        {
            await _repository.DeleteById(id);
        }
    }
}


