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
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly TransferRepository _repository;
        public TransferController(TransferRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<IEnumerable<TransferModel>>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetId(id);
            if (response.Count > 0)
            {
                msg.IsSuccess = true;
                msg.Data = response;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no asset found";
            }
            return Ok(msg);
        }

        // POST api/values
        [HttpGet("GetAllTransfers")]
        public async Task<ActionResult<IEnumerable<TransferModel>>> SearchTransfers ([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] int id=0)//, [FromQuery] string ptype=null, [FromQuery] string mtype=null, [FromQuery] string rtype =null, [FromQuery] string btype=null)
        {
            var msg = new Message();
            var transfers = await _repository.SearchTransfers(pageNumber, pageSize,id);//,ptype,mtype,rtype,btype);
            if (transfers.Count > 0)
            {
                msg.IsSuccess = true;
                msg.Data = transfers;


            }

            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values found";
            }
                return Ok(msg);
        }






        //[HttpPost("AddNew")]
        //public async Task<IActionResult> Post([FromBody] TransferModel asset)
        //{

        //    var msg = new Message();
        //    await _repository.Insert(asset);
        //    bool exists = _repository.Itexists;
        //    bool success = _repository.IsSuccess;

        //    if (exists is true)
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "Item alredy registered";
        //    }
        //    else if (success is true)
        //    {
        //        msg.IsSuccess = true;
        //        msg.ReturnMessage = " new entry succesfully registered";
        //    }
        //    else
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "registeration unscessfull";
        //    }
        //    return Ok(msg);
        //}

        // PUT api/values/
    }

}
