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
    [Authorize(Roles = "Admin")]
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
        [HttpGet("GetAllDep")]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> GetAllDep([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var msg = new Message();
            var dep = await _repository.GetAllDep(PageNumber, PageSize);
            if (dep.Count>0) { msg.Data = dep; } else { msg.ReturnMessage = "No values found"; }
            return Ok(msg);
            
        }
        //    public async Task<ActionResult<IEnumerable<DepartmentModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> SearchDep([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null,[FromQuery]int Dep=0)
        {
            var msg = new Message();
            var assets = await _repository.SearchDepartment(pageNumber, pageSize, searchTerm,Dep);
            if (assets.Count > 0) { msg.Data = assets; }else{ msg.ReturnMessage = "No id found"; }

            return Ok(msg);
        }
        // GET api/values/5
        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<DepartmentModel>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetById(id);
            if (response.Count>0) {
                msg.IsSuccess = true;
                msg.Data = response; }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "No id found";
            }
            return Ok(msg);
        }

        // POST api/values
        [HttpPost("AddNew")]
        public async Task<IActionResult> Post([FromBody] DepartmentModel dep)
        {
            var msg = new Message();
            await _repository.Insert(dep);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.ReturnMessage = "Item alredy registered";
            }
            else if (success is true)
            {
                msg.ReturnMessage = " new asset succesfully registered";
            }
            else
            {
                msg.ReturnMessage = "registeration unscessfull";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update( [FromBody] DepartmentModel dep, int id)
        {
            var msg = new Message();
            var GetComp = await _repository.GetById(id);
            if (GetComp.Count>0)
            {
                await _repository.UpdateDep(dep, id);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.ReturnMessage = " updated succefully";
                }
                else
                {
                    msg.ReturnMessage = "  update unsuccessfull ";
                }
               
            }
            else
            {
                msg.ReturnMessage = "no value found";

            }
            return Ok(msg);
        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id=0)
        {
            var msg = new Message();
            var GetDep = await _repository.GetById(id);

            if (GetDep.Count > 0)
            {

                await _repository.DeleteById(id);
                msg.ReturnMessage = "Succesfully Deleted";
            }
            else
            {
                msg.ReturnMessage = "no values found";
            }

            return Ok(msg);
           // await _repository.DeleteById(id);
        }
    }
}


