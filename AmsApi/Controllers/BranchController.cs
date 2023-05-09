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
    //[AllowAnonymous]
    [Authorize]//(Roles = "Admin")
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
        //[HttpGet("GetAllBranch")]
        //public async Task<ActionResult<IEnumerable<BranchModel>>> GetAllBranch([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        //{
        //    var msg = new Message();
        //    var branch = await _repository.GetAllBranch(PageNumber, PageSize);
        //    if (branch.Count > 0)
        //    {
        //        msg.IsSuccess = true;
        //        msg.Data = branch;
                
        //    }
        //    else 
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "no brnaces found";
               
        //    }
        //    return Ok(msg);

        //}
        //public async Task<ActionResult<IEnumerable<BranchModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("GetAllBranch")]
        public async Task<ActionResult<IEnumerable<BranchModel>>> SearchBranch([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int brcid = 0)
        {
            var msg = new Message();
            var branches = await _repository.SearchBranches(pageNumber, pageSize, searchTerm, brcid);
            if (branches.Count > 0) {
                msg.IsSuccess = true;
                msg.Data = branches;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no match found";
            }

            return Ok(msg);

            }
        
        // GET api/values/5
            [HttpGet("Getid/{id}")]
        public async Task<ActionResult<IEnumerable<BranchModel>>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetById(id);
            if (response.Count>0) {
                msg.IsSuccess = true;
                msg.Data=response;
            }
            else {
                msg.IsSuccess = false;
                
                msg.ReturnMessage = "no id found"; }
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
                msg.ItExists = true;
                msg.IsSuccess = false;
                msg.ReturnMessage = "  entry already exists";
            }
            else if(success is true)
                {
                msg.IsSuccess = true;
                    msg.ReturnMessage = " new branch succesfully registered";
                }
              
            else{
                msg.IsSuccess = false;
                msg.ReturnMessage = "registeration unscessfull";// mostly server side error 500 series or other reasons
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPost("Update")]
        public async Task<IActionResult> Update( [FromBody] BranchModel branch)
        {
            var msg = new Message();
         
            //var GetBranch = await _repository.GetBranchById(branch);
            //if (GetBranch.Count>0)
            //{
                await _repository.UpdateBranch(branch);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = " updated successfully";
                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = " updated unsuccessfull";
                }
               
            //}
            //else
            //{
            //    msg.IsSuccess = false;
            //    msg.ReturnMessage = "no branch found";
            //}

            
            return Ok(msg);

        }

        // DELETE api/values/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id=0)
        {
            var msg = new Message();
            var GetBranch = await _repository.GetById(id);
            if (GetBranch.Count> 0)
            {
                await _repository.DeleteById(id);
                msg.IsSuccess = true;
                msg.ReturnMessage = "succesfully removed";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "removal unsuccessfull";
            }
            return Ok(msg);
        }
    }
}


