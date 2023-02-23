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
    public class BranchController : ControllerBase
    {
        private readonly BranchRepository _repository;
        public BranchController(BranchRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet("GetAllBranch")]
        public async Task<ActionResult<IEnumerable<BranchModel>>> GetAllBranch([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var msg = new Message();
            var branch = await _repository.GetAllBranch(PageNumber, PageSize);
            if (branch.Count > 0)
            {
                return branch;
            }
            else {
                msg.ReturnMessage = "no brnaces found";
                return Ok(msg); }
           
        }
        //public async Task<ActionResult<IEnumerable<BranchModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<BranchModel>>> SearchBranch([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int brcid = 0)
        {
            var msg = new Message();
            var assets = await _repository.SearchBranches(pageNumber, pageSize, searchTerm, brcid);
            if (assets.Count > 0) {
                return assets;
            }
            else { msg.ReturnMessage = " no match found";  }

            return Ok(msg);

            }
        
        // GET api/values/5
            [HttpGet("Getid/{id}")]
        public async Task<ActionResult<IEnumerable<BranchModel>>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetById(id);
            if (response.Count>0) {
                return response;
            }
            else { msg.ReturnMessage = "no id found"; }
            return Ok(msg);
        }

        // POST api/values
        [HttpPost("AddNew")]
        public async Task<IActionResult> Post([FromBody] BranchModel value)
        {
            var msg = new Message();
            await _repository.Insert(value);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;
            if (exists is true)
            {
                msg.ReturnMessage = "  entry already exists";
            }
            else if(success is true)
                {
                    msg.ReturnMessage = " new branch succesfullly registered";
                }
              
            else{
                msg.ReturnMessage = "registeration unscessfull";// mostly server side error 500 series or other reasons
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update( [FromBody] BranchModel branch, int id=0)
        {
            var msg = new Message();
         
            var GetBranch = await _repository.GetBranchById(id);
            if (GetBranch.Count>0)
            {
                await _repository.UpdateBranch( branch, id);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.ReturnMessage = " updated successfully";
                }
               
            }
            else
            {
                msg.ReturnMessage = "no brance found";
            }

            
            return Ok(msg);

        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id=0)
        {
            var msg = new Message();
            var GetBranch = await _repository.GetById(id);
            if (GetBranch.Count> 0)
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


