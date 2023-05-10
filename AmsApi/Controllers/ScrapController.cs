using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using AmsApi.Repository;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmsApi.Controllers
{
    [AllowAnonymous]
    //[Authorize]// (Roles = "Admin")
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapController : ControllerBase
    {
        private readonly ScrapRepository _repository;
        public ScrapController(ScrapRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        //[HttpGet("GetAllTables")]
        //public IActionResult GetAllTables()
        //{
        //    var result = _repository.GetAllTables();
        //    var msg = new Message();
        //    if (result is null)
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "no values available";
        //    }
        //    else
        //    {
        //        msg.IsSuccess = true;
        //        msg.Data = result;
        //    }


        //    return Ok(msg);
        //}

        //[HttpGet("GetAllScrap")]

        //public async Task<ActionResult<IEnumerable<ScrapModel>>> GetAllScrap([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        //{
        //    var msg = new Message();
        //    var scrap = await _repository.GetAllScrap(PageNumber, PageSize);
        //    if (scrap.Count>0) { 
        //        msg.IsSuccess = true;
        //        msg.Data = scrap;
        //    }
        //    else
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "no scraps found";
        //    }
        //    return Ok(msg);
        //}
        ////public async Task<ActionResult<IEnumerable<ScrapModel>>> Get()
        //{
        //    return await _repository.GetAllScraps();
        //}
        [HttpGet("GetAllScrap")]
        //public async Task<ActionResult<IEnumerable<ScrapModel>>> SearchScrap([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int searchId = 0, [FromQuery] int assetId = 0, [FromQuery] int brcId = 0, [FromQuery] int vedId = 0, [FromQuery] int userid = 0,[FromQuery] int DateFilter=0,[FromQuery] string StartDate=null,[FromQuery] string EndDate = null)
        //{
        //    var msg = new Message();
        //    var Scrap = await _repository.SearchScrap(pageNumber, pageSize, searchTerm, searchId, assetId, brcId, vedId, userid, DateFilter,StartDate,EndDate);
        //    if (Scrap.Count>0) { 
        //        msg.IsSuccess = true;
        //        msg.Data = Scrap;
        //    }
        //    else
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "No Matches found ";
        //    }
        //    return Ok(msg); 
        //}

        public async Task<ActionResult<IEnumerable<ReportModel>>> GetScrapsTable([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5,[FromQuery] string searchString=null, [FromQuery] int brcid=0, [FromQuery] int typ=0)
        {
            var msg = new Message();
            var Scrap = await _repository.GetScrapsTable( pageNumber, pageSize,searchString, brcid, typ);
            if (Scrap.Count > 0)
            {
                msg.IsSuccess = true;
                msg.Data = Scrap;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "No Matches found ";
            }
            return Ok(msg);
        }



        // GET api/values/5
        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<ScrapModel>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetId(id);
            if (response.Count > 0)
            {
                msg.IsSuccess = true; msg.Data = response;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " no id found";
            }

            return Ok(msg);
        }

        // POST api/values
        [HttpPost("AddNew")]
        public async Task<ActionResult> Post([FromBody] ScrapModel request)
        {
            var msg = new Message();
            await _repository.Insert(request);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.ItExists = true;
                msg.IsSuccess = false;
                msg.ReturnMessage = "item already registered";
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
        public async Task<IActionResult> Update([FromBody] ScrapModel scrap)
        {
            var msg = new Message();
            //var GetScrap = await _repository.GetScrapId(scrap);
            //if (GetScrap.Count > 0)
            //{
                await _repository.UpdateScrap(scrap);
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
            //}
            //else
            //{
            //    msg.IsSuccess = false;
            //    msg.ReturnMessage = "No  entry found";

            //}


            return Ok(msg);

        }

        // DELETE api/values/5
        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var msg = new Message();
            var GetScrap = await _repository.GetId(id);
            if (GetScrap.Count > 0)
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
