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
    public class UserDetailsController : ControllerBase
    {
        private readonly UserDetailsRepository _repository;
        public UserDetailsController(UserDetailsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailsModel>>> GetAllDetails([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var details = await _repository.GetAllDetails(PageNumber, PageSize);
            if (details == null) { return NotFound(); }
            return details;
        }
        //public async Task<ActionResult<IEnumerable<UserDetailsModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<UserDetailsModel>>> SearchUserDetails([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int searchId = 0, [FromQuery] int depId = 0, [FromQuery] int brcId = 0, [FromQuery] int compId = 0, [FromQuery] int userId = 0, [FromQuery] int floor = 0)
        {
            var udetails = await _repository.SearchUserDetails(pageNumber, pageSize, searchTerm,searchId, depId, brcId, compId, userId, floor);
            if (udetails == null) { return NotFound(); }
            return udetails;
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailsModel>> Get(int id)
        {
            var response = await _repository.GetById(id);
               if (response == null) { return NotFound(); }
            return response;
            
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] UserDetailsModel details)
        {
            await _repository.Insert(details);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDetailsModel details)
        {
            var GetComp = await _repository.GetById(id);
            if (GetComp == null)
            {
                return NotFound();
            }

            await _repository.UpdateUserDetails(details, id);

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


