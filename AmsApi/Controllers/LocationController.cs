using System;
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
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        private readonly LocationRepository _repository;
        public LocationController(LocationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<locationController>
        //[HttpGet("GetLocations")]
        //public async Task<ActionResult<IEnumerable<LocationModel>>> GetAllLocations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5,[FromQuery] int lid = 0, [FromQuery] int aid = 0, [FromQuery] int tid = 0, [FromQuery] int uid = 0, [FromQuery] int bid = 0, [FromQuery] int cid = 0, [FromQuery] int did = 0, [FromQuery] int rid = 0,[FromQuery] int f = 0)
        //{
        //    var msg = new Message();
        //    var locs = await _repository.GetAllLocations_Paginated(pageNumber, pageSize,lid,aid,tid,uid,bid,cid,did,rid,f);
        //    if (locs.Count>0)
        //    {
        //        msg.IsSuccess = true;
        //        msg.Data = locs;
        //    }

        //    else
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "no value found";
        //    }
        //    return Ok(msg);
        //}
        [HttpGet("GetLocations")]
        public async Task<ActionResult<IEnumerable<LocationModel>>> SearchAllLocations([FromQuery] string Searchterm = null,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] int lid = 0, [FromQuery] int aid = 0, [FromQuery] int tid = 0, [FromQuery] int uid = 0, [FromQuery] int bid = 0, [FromQuery] int cid = 0, [FromQuery] int did = 0, [FromQuery] int rid = 0, [FromQuery] int f = 0)
        {
            var msg = new Message();
            var locs = await _repository.SearchAllLocations_Paginated(Searchterm, pageNumber, pageSize, lid, aid, tid, uid, bid, cid, did, rid, f);
            if (locs.Count > 0)
            {
                msg.IsSuccess = true;
                msg.Data = locs;
            }

            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no value found";
            }
            return Ok(msg);
        }


        // GET api/<locationController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<locationController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<locationController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<locationController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
