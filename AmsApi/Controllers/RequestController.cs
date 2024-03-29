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
    // [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestRepository _repository;
        public RequestController(RequestRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // [Authorize("Admin")]
        //[HttpGet("GetAllRequests")]

        //public async Task<ActionResult<IEnumerable<RequestModel>>> GetAllRequests([FromQuery]int pageNumber=1,[FromQuery] int pageSize=5)
        //{
        //    var msg = new Message();
        //    var request = await _repository.GetAllRequests(pageNumber,pageSize);
        //    if(request.Count>0) {
        //        msg.IsSuccess = true;
        //        msg.Data = request; } 
        //    else
        //    {
        //        msg.IsSuccess =false;
        //        msg.ReturnMessage = "no request found";
        //    }
        //    return Ok(msg);
        //}
        //   [Authorize("Admin")]
        [HttpGet("GetAllRequests")]
        public async Task<ActionResult<IEnumerable<RequestModel>>> SearchRequests([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchString = null, [FromQuery] int userId = 0, [FromQuery] int reqId = 0, [FromQuery] int assetId = 0, [FromQuery] int statId = 0,[FromQuery] int DateFilter=0,[FromQuery] string StartDate=null,[FromQuery] string EndDate=null)
            {
            var msg = new Message();
            var requests = await _repository.SearchRequests(pageNumber, pageSize, searchString, userId, reqId, assetId, statId, DateFilter,StartDate,EndDate);
            if (requests.Count > 0) {
                msg.IsSuccess = true;
                msg.Data = requests;
            } else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " no match found";
            }
            return Ok(msg);
        }
        // GET api/values/5
        // [Authorize("Admin,User")]
        [HttpGet("Getbyid/{id}")]
        public async Task<ActionResult<RequestModel>> Get(int id = 0)//will be used to display all the request done in by the user
        {
            var msg = new Message();
            var response = await _repository.GetRequestId(id);
            if (response.Count > 0)
            {
                msg.IsSuccess = true;
                msg.Data = response;
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values found";
            }
            return Ok(msg);
        }

        /*
         * if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
*/
        [HttpGet("AssetDropdown/{id}")]
        public IActionResult Getdropdownid(int id = 0)//will be used to display all the request done in by the user
        {
            var msg = new Message();
            var response = _repository.Getassetdropdown(id);
            if (response is null)
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values available";
            }
            else
            {
                msg.IsSuccess = true;
                msg.Data = response;
            }


            return Ok(msg);
        }
        //[Authorize("User")]
        // POST api/values
        [HttpPost("CreateNew")]
     //   public async Task<IActionResult> Post([FromBody] RequestModel request)
        public async Task<IActionResult> Post(int userid,int asset,string justify)//,int type
        {

            var msg = new Message();
            await _repository.Insert(userid,asset,justify);//,type
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;

            if (exists is true)
            {
                msg.IsSuccess = false;
                msg.ItExists = true;
                msg.ReturnMessage = "request alredy registered";
            }
            else if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = " new request succesfully registered";

            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "request unscessfull";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        // [Authorize("Admin,User")]
        [HttpPost("UpdateRequest")]
        public async Task<IActionResult> Update(int id, int userid, int asset, string justify)//,int type
        {

            var msg = new Message();
            //var GetRequest = await _repository.GetRequestId(id);
            //if (GetRequest.Count > 0)
            //{
                await _repository.UpdateRequest(userid,asset,justify,id);//,type
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "request updated successfully";
                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "updated unsuccessfull";
                }
            //}
            //else
            //{
            //    msg.IsSuccess = false;
            //    msg.ReturnMessage = "no id found";
            //}


            return Ok(msg);
        }
        [HttpGet("RequestAccepted/{id}")]
        public async Task<IActionResult> RequestAccepted(int id, [FromQuery] int type = 0, [FromQuery] bool isAccepted = false)
        {

            var msg = new Message();
            var GetRequest = await _repository.GetRequestId(id);
            if (GetRequest.Count > 0)
            {
                await _repository.RequestAccepted(isAccepted, type, id);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "request updated successfully";
                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "updated unsuccessfull";
                }
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no id found";
            }


            return Ok(msg);
        }

        [HttpGet("AssetAllotment/{id}")]
        public async Task<IActionResult> AssetAllotment(int id,[FromQuery] string UniqueId, [FromQuery] string SerialNo)
        {

            var msg = new Message();
            var GetRequest = await _repository.GetRequestId(id);
            if (GetRequest.Count > 0)
            {
                await _repository.UniqueIdAdd(UniqueId,SerialNo,id);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "request updated successfully";
                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "updated unsuccessfull";
                }
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no id found";
            }


            return Ok(msg);
        }


        [HttpGet("StatusChange/{id}")]
        public async Task<IActionResult> StatusChange(int id, [FromQuery] int type=0, [FromQuery] bool isworking=true, [FromQuery] bool inuse=false)//, [FromQuery] string UniqueId =null, [FromQuery] string SerialNo =null
        {

            var msg = new Message();
            var GetRequest = await _repository.GetRequestId(id);
            if (GetRequest.Count > 0)
            {
                await _repository.StatusChange(isworking,inuse,type, id);//UniqueId,SerialNo,
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "request updated successfully";
                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "updated unsuccessfull";
                }
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no id found";
            }


            return Ok(msg);
        }

        [HttpGet("SendtoScrap/{id}")]
        public async Task<IActionResult> SendtoScrap(int id, [FromQuery]string Uid,[FromQuery] int type = 0, [FromQuery] bool isScrap = false)
        {

            var msg = new Message();
            //var GetRequest = await _repository.GetRequestId(id);
            //if (GetRequest.Count > 0)
            //{
                await _repository.SendtoScrap(isScrap, type,Uid,id);
                bool success = _repository.IsSuccess;
                if (success is true)
                {
                    msg.IsSuccess = true;
                    msg.ReturnMessage = "Sent for maintainence successfully";
                }
                else
                {
                    msg.IsSuccess = false;
                    msg.ReturnMessage = "request unsuccessfull";
                }
            //}
            //else
            //{
            //    msg.IsSuccess = false;
            //    msg.ReturnMessage = "no id found";
            //}

            return Ok(msg);
        }

        [HttpGet("SentForFix/{id}")]
        public async Task<IActionResult> SentForFix( int id,[FromQuery] int type = 0, [FromQuery] bool SentForFix = false, [FromQuery] int Vendorid=0, [FromQuery] string Uid=null, [FromQuery] string Description=null)// [FromQuery] string Uid,
        {

            var msg = new Message();
            //var GetRequest = await _repository.GetRequestId(id);
            //if (GetRequest.Count > 0)
            //{
            await _repository.SentForFix(SentForFix, type,Vendorid, Uid, Description, id);//, Uid
            bool success = _repository.IsSuccess;
            if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = "sent for Maintainence ";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "update unsuccessfull";
            }
            //}
            //else
            //{
            //    msg.IsSuccess = false;
            //    msg.ReturnMessage = "no id found";
            //}

            return Ok(msg);
        }

        // DELETE api/values/5
        // [Authorize("Admin,User")]
        [HttpGet("DeleteRequest/{id}")]
        public async Task<IActionResult> Delete(int id =0)
        {
            var msg = new Message();
            //await _repository.DeleteById(id);
         
            var GetComp = await _repository.GetRequestId(id);

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


