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

namespace AmsApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyRepository _repository;
        public CompanyController(CompanyRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> GetAllComp([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var Comp = await _repository.GetAllComp(PageNumber, PageSize);
            if (Comp == null) { return NotFound(); }
            return Comp;
        }
        //public async Task<ActionResult<IEnumerable<CompanyModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> SearchComp([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null,[FromQuery]int Comp=0)///[FromQuery]int Comp=0 )
        {
            var Company = await _repository.SearchCompany(pageNumber, pageSize, searchTerm,Comp);
            if (Company == null) { return NotFound(); }
            return Company;
        }
        // GET api/values/5
        //Search/
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> Get(int id)
        {
            var msg = new Message<CompanyModel>();
            var response = await _repository.GetById(id);
            if (response.Count>0) { return response; }
            else { msg.ReturnMessage = "no companies found"; }
            return Ok(msg);
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] CompanyModel company)
        {
            await _repository.Insert(company);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyModel comp)
        {
            var GetComp = await _repository.GetById(id);
            if (GetComp == null)
            {
                return NotFound();
            }

            await _repository.UpdateComp(comp, id);

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


