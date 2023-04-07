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
    //[Authorize(Roles = "Admin")]
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
        //[HttpGet("GetAllVendors")]
        //public async Task<ActionResult<IEnumerable<VendorModel>>> GetAllVendors(int PageNumber=1,int PageSize=5)
        //{
        //    var msg = new Message();
        //    var Vendors = await _repository.GetAllVendors(PageNumber, PageSize);
        //    if (Vendors.Count>0) {
        //        msg.IsSuccess = true;
        //        msg.Data = Vendors;
        //          }
        //    else { msg.IsSuccess = false; 
        //        msg.ReturnMessage=" no vendors registered"; }
        //    return Ok(msg);
        //}
        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<IEnumerable<VendorModel>>> Get(VendorModel v)
        {
            var msg = new Message();
            var GetVendor = await _repository.GetById(v);
            if (GetVendor.Count > 0)
            {
                msg.IsSuccess = true;
                msg.Data = GetVendor;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " no vendors registered";
            }
            return Ok(msg);
        }
        // GET api/values/5
        [HttpGet("GetAllVendors")]
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
        [HttpPost("AddNew")]
        public async Task<IActionResult> Post([FromBody] VendorModel vendor)
        {
            var msg = new Message();
            await _repository.Insert(vendor);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.ItExists = true;
                msg.IsSuccess = false;
                msg.ReturnMessage = "vendor already registered";
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
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] VendorModel vendor)
        {
            var msg = new Message();
            var GetVendors = await _repository.GetById(vendor);
            if (GetVendors.Count > 0)
            {

                await _repository.UpdateVendor(vendor);
                bool success = _repository.IsSuccess;
                
                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "values updated successfully";
                }
                else
                {
                    msg.IsSuccess = false;
                   
                    msg.ReturnMessage = " update unsuccessfull";
                }
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "No Vendor entries found";

            }


            return Ok(msg);

        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var msg = new Message();
            var GetVendors = await _repository.GetDetId(id);
            if (GetVendors.Count > 0)
            {
                await _repository.DeletevendorById(id);
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


