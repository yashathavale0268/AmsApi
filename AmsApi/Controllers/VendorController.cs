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
            var msg = new Message<UserModel>();
            var Vendors = await _repository.GetAllVendors(PageNumber, PageSize);
            if (Vendors == null) { msg.ReturnMessage="No entries found";  }
            else { return Vendors; }
            return Ok(msg);
        }

        // GET api/values/5
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<VendorModel>>> SearchVendors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null,[FromQuery]String InvDate=null,[FromQuery] String WarryTillDate = null)
        {
            var msg = new Message<UserModel>();
            var Vendors = await _repository.SearchVendors(pageNumber, pageSize, searchTerm,InvDate,WarryTillDate);
            if (Vendors == null) { msg.ReturnMessage = "No entries found"; }
            
            return Vendors;
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] VendorModel vendor)
        {
            await _repository.Insert(vendor);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VendorModel vendor)
        {
            var msg = new Message<UserModel>();
            var GetVendors = await _repository.GetById(id);
            if (GetVendors == null)
            {
                msg.ReturnMessage="No Vendor entries found";
            }

            await _repository.UpdateVendor(id, vendor);

            return Ok(msg);

        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task Delete(int id)
        {
            await _repository.DeletevendorById(id);
        }
    }
}


