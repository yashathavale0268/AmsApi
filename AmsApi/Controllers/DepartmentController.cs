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
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentRepository _repository;
        public DepartmentController(DepartmentRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> GetAllDep([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var dep = await _repository.GetAllDep(PageNumber, PageSize);
            if (dep == null) { return NotFound(); }
            return dep;
        }
        //    public async Task<ActionResult<IEnumerable<DepartmentModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> SearchDep([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null,[FromQuery]int Dep=0)
        {
            var assets = await _repository.SearchDepartment(pageNumber, pageSize, searchTerm,Dep);
            if (assets == null) { return NotFound(); }
            return assets;
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentModel>> Get(int id)
        {
            var response = await _repository.GetById(id);
            if (response == null) { return NotFound(); }
            return response;
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] DepartmentModel dep)
        {
            await _repository.Insert(dep);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentModel dep)
        {
            var GetComp = await _repository.GetById(id);
            if (GetComp == null)
            {
                return NotFound();
            }

            await _repository.UpdateDep(dep, id);

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


