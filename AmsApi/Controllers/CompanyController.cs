﻿using System;
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
    //[Authorize(Roles = "Admin")]
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
        //[HttpGet("GetAllComp")]
        //public async Task<ActionResult<IEnumerable<CompanyModel>>> GetAllComp([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        //{
        //    var msg = new Message();
        //    var Comp = await _repository.GetAllComp(PageNumber, PageSize);
        //    if (Comp.Count>0)
        //    {
        //        msg.IsSuccess=true;
        //        msg.Data = Comp;
        //    }
        //    else
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "no values found";
        //    }
        //    return Ok(msg);
        //}
        //public async Task<ActionResult<IEnumerable<CompanyModel>>> Get()
        //{
        //    return await _repository.GetAll();
        //}
        [HttpGet("GetAllComp")]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> SearchComp([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null,[FromQuery]int Comp=0)///[FromQuery]int Comp=0 )
        {
            var msg = new Message();
            var Company = await _repository.SearchCompany(pageNumber, pageSize, searchTerm,Comp);
            if (Company.Count>0)
            {
                msg.IsSuccess = true;
                msg.Data = Company;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "No match found";
            }
            return Ok(msg);
        }
        // GET api/values/5
        //Search/
        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetId(id);
            if (response.Count>0) 
            {
                msg.IsSuccess = true;
                msg.Data = response; 
            }
            else 
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no id found"; 
            }
            return Ok(msg);
        }

        // POST api/values
        [HttpPost("AddNew")]
        public async Task<IActionResult> Post([FromBody] CompanyModel company)
        {
            var msg = new Message();
            await _repository.Insert(company);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.ItExists = true;
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

        // PUT api/values/5
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] CompanyModel comp)
        {
            var msg = new Message();
            //var GetComp = await _repository.GetById(comp);
            //if (GetComp.Count>0)
            //{
                await _repository.UpdateComp(comp);
                msg.IsSuccess = true;
                //msg.ReturnMessage = " update successfull";
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
            return Ok(msg);
        }

        // DELETE api/values/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id=0)
        {
            var msg = new Message();
            var GetComp = await _repository.GetId(id);
         
            if (GetComp.Count>0)
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
        }
    }
}


