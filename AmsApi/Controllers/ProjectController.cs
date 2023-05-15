using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectRepository _repository;
        public ProjectController(ProjectRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        [HttpGet("GetAllProjects")]
        public IActionResult Getproject([FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 0, [FromQuery] string searchTerm = null)
        {
            var msg = new Message();
            var GetDets = _repository.SearchProject(pageNumber, pageSize, searchTerm);
            if (GetDets.Tables.Count > 0)
            {
                msg.IsSuccess = true;
                msg.Data = GetDets;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values found";
            }
            return Ok(msg);
        }
        [HttpGet("{id}")]
        public IActionResult Getprojectid([FromQuery] int id)
        {
            var msg = new Message();
            var GetDets = _repository.GetProjectid(id);
            if (GetDets.Tables.Count > 0)
            {
                msg.IsSuccess = true;
                msg.Data = GetDets;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values found";
            }
            return Ok(msg);
        }

        // POST api/<ProjectController>
        [HttpPost]
        public IActionResult Post(ProjectModel proj)
        {
            var msg = new Message();
             _repository.Insert(proj);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "Item alredy registered";
            }
            else if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = " new entry succesfully registered";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "registeration unscessfull";
            }
            return Ok(msg);
        }

        //// PUT api/<ProjectController>/5
        [HttpPut("{id}")]
        public IActionResult Put(ProjectModel proj)
        {
            var msg = new Message();
            _repository.Insert(proj);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "Item alredy registered";
            }
            else if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = " new entry succesfully registered";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "registeration unscessfull";
            }
            return Ok(msg);
        }

        //// DELETE api/<ProjectController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var msg = new Message();

               _repository.DeleteById(id);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;
            if (exists is false)
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "entry doesn't exist";
            }
            else if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = "succesfully removed";
            }
            else
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = "removal unsuccessfull";
            }
            return Ok(msg);
        }
    }
}
