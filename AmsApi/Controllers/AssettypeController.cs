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
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssettypeController : ControllerBase
    {
        private readonly AssettypeRepository _repository;
        public AssettypeController(AssettypeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet("GetAllTypes")]
        public async Task<ActionResult<IEnumerable<AssettypeModel>>> GetAllTypes([FromQuery] int PageNumber=1, [FromQuery] int PageSize=5)
        {
            var msg = new Message();
            var type = await _repository.GetAllTypes(PageNumber, PageSize);
            if (type == null) {
                msg.ReturnMessage = "no types found";
                return Ok(msg); }
            return type;
        }
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<AssettypeModel>>> SearchTypes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery]int typeid =0)
        {
            var msg = new Message();
            var assets = await _repository.SearchTypes(pageNumber, pageSize, searchTerm,typeid);
            if (assets.Count>0) {  return assets; } else
            {
                msg.ReturnMessage = "no types found";
            }
            
            return Ok(msg);

        }
            // GET api/values/5
            [HttpGet("Getid/{id}")]
        public async Task<ActionResult<IEnumerable<AssettypeModel>>> Get(int id)
        {
            var msg = new Message();
            var response =
            await _repository.GettypeById(id);
            if (response.Count>0) {
                msg.IsSuccess = true;
                msg.Data=response; } else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no types found";
            }

            //return response;
            return Ok(msg);
        }

        // POST api/values
        [HttpPost("AddNew")]
        public async Task<IActionResult> Post([FromBody] AssettypeModel type)
        {
            var msg = new Message();
            await _repository.Insert(type);
           bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;
            if (exists is true)
            {
                    msg.ReturnMessage = "value already exists";
            }
            else if(success is true)
            {
               
                msg.ReturnMessage = "successfully submitted";
            }
            else
            {
                msg.ReturnMessage = "unsccessfull insert";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update( [FromBody] AssettypeModel type,int id= 0)
        {
            var msg = new Message();
            var Gettype = await _repository.GettypeById(id);
            if (Gettype.Count>0)
            {
                await _repository.UpdateType(type, id);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.ReturnMessage = " updated successfully";
                }
                else
                {
                    msg.ReturnMessage = " update unsuccessfull";
                }

            }
            else
            {
                msg.ReturnMessage = "no type found";
            }


            return Ok(msg);

        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id=0)
        {
            var msg = new Message();
            var gottype = await _repository.GettypeById(id);

            if (gottype.Count> 0)
            {

                await _repository.DeletetypeById(id);
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


