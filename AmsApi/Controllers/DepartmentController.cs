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
   // [Authorize(Roles = "Admin")]
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
        //[HttpGet("GetAllDep")]
        //public async Task<ActionResult<IEnumerable<DepartmentModel>>> GetAllDep([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        //{
        //    var msg = new Message();
        //    var dep = await _repository.GetAllDep(PageNumber, PageSize);
        //    if (dep.Count>0) { msg.IsSuccess = true;
        //        msg.Data = dep; } else {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "No values found"; }
        //    return Ok(msg);
            
        //}
        //    public async Task<ActionResult<IEnumerable<DepartmentModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("GetAllDep")]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> SearchDep([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null,[FromQuery]int Dep=0)
        {
            var msg = new Message();
            var assets = await _repository.SearchDepartment(pageNumber, pageSize, searchTerm,Dep);
            if (assets.Count > 0) {
                msg.IsSuccess = true;
                msg.Data = assets; }else{
                msg.IsSuccess = false;
                msg.ReturnMessage = "No id found"; }

            return Ok(msg);
        }
        // GET api/values/5
        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<DepartmentModel>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetId(id);
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
                msg.ItExists = true;
                msg.IsSuccess = false;
                msg.ReturnMessage = "Entry alredy registered";
            }
            else if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = " New Entry succesfully registered";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "Registeration unscessfull";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPost("Update")]
        public async Task<IActionResult> Update( [FromBody] DepartmentModel dep)
        {
            var msg = new Message();
            //var GetComp = await _repository.GetById(dep);
            //if (GetComp.Count>0)
            //{
                await _repository.UpdateDep(dep);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = " updated succefully";
                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "  update unsuccessfull ";
                }
               
            //}
            //else
            //{
            //    msg.IsSuccess = false;
            //    msg.ReturnMessage = "no value found";

            //}
            return Ok(msg);
        }

        // DELETE api/values/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id=0)
        {
            var msg = new Message();
            var GetDep = await _repository.GetId(id);

            if (GetDep.Count > 0)
            {

                await _repository.DeleteById(id);
                msg.IsSuccess = true;
                msg.ReturnMessage = "Succesfully Deleted";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values found";
            }

            return Ok(msg);
           // await _repository.DeleteById(id);
        }
    }
}


