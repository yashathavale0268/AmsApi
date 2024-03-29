﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using AmsApi.Repository;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerInfoController : ControllerBase
    {
        private readonly ServerInfoRepository _repository;
        public ServerInfoController(ServerInfoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        // GET: api/<ServerInfoController>
        [HttpGet("GetRolePerms")]
        public IActionResult GetRolePerms( int User = 0,  int Menu=0)
        {
            var msg = new Message();
            var GetDets = _repository.GetRolePerms(User,Menu);
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

        
        // GET: api/<ServerInfoController>
        [HttpGet("GetAllServerInfo")]
        public IActionResult GetServerInfo([FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 0, [FromQuery] string searchTerm = null)
        {
            var msg = new Message();
            var GetDets = _repository.SearchServerInfo(pageNumber, pageSize, searchTerm);
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

        // GET api/<ServerInfoController>/5
        [HttpGet("{id}")]
        public IActionResult GetServerInfoid(int id)
        {
            var msg = new Message();
            var GetDets = _repository.GetServerInfo(id);
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

        // POST api/<ServerInfoController>
        [HttpPost]
        public IActionResult Post(ServerInfoModel serv)
        {
            var msg = new Message();
            _repository.Insert(serv);
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

        // PUT api/<ServerInfoController>/5
        [HttpPut]
        public IActionResult Put(ServerInfoModel serv)
        {
            var msg = new Message();
            _repository.Insert(serv);
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
                msg.ReturnMessage = " update successful";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "registeration unsucessfull";
            }
            return Ok(msg);
        }

        // DELETE api/<ServerInfoController>/5
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
