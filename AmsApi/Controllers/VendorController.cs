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

namespace AmsApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly VendorRepository _repository;
        public VendorController(VendorRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendorModel>>> GetAllVendors(int PageNumber=1,int PageSize=5)
        {
            var msg = new Message();
            var Vendors = await _repository.GetAllVendors(PageNumber, PageSize);
            if (Vendors == null) { msg.ReturnMessage="No entries found";  }
            else { return Vendors; }
            return Ok(msg);
        }

        // GET api/values/5
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<VendorModel>>> SearchVendors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null,[FromQuery]String InvDate=null,[FromQuery] String WarryTillDate = null)
        {
            var msg = new Message();
            var Vendors = await _repository.SearchVendors(pageNumber, pageSize, searchTerm,InvDate,WarryTillDate);
            if (Vendors.Count>0) {
                msg.IsSuccess = true;
                msg.Data = Vendors;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "No entries found";
            }
            
             return Ok(msg);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VendorModel vendor)
        {
            var msg = new Message();
            await _repository.Insert(vendor);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.ReturnMessage = "vendor already registered";
            }
            else if (success is true)
            {
                msg.ReturnMessage = " new entry succesfully registered";
            }
            else
            {
                msg.ReturnMessage = "registeration unscessfull";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] VendorModel vendor, int id=0)
        {
            var msg = new Message();
            var GetVendors = await _repository.GetById(id);
            if (GetVendors.Count > 0)
            {

                await _repository.UpdateVendor(vendor, id);
                bool success = _repository.IsSuccess;
                
                if (success is true)
                {
                    msg.ReturnMessage = "values updated successfully";
                }
                else
                {
                    msg.ReturnMessage = " update unsuccessfull";
                }
            }
            else
            {

                msg.ReturnMessage = "No Vendor entries found";

            }


            return Ok(msg);

        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var msg = new Message();
            var GetVendors = await _repository.GetById(id);
            if (GetVendors.Count > 0)
            {
                await _repository.DeletevendorById(id);
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


