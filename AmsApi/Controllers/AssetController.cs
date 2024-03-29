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
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AmsApi.Controllers
{
    [AllowAnonymous]
    //[Authorize(Roles ="Admin")]//AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly AssetRepository _repository;
        public AssetController(AssetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        //public async Task<ActionResult<IEnumerable<AssetModel>>> Get()
        //{
        //    return await _repository.GetAll();

        //}
       
        [HttpGet("GetAllTables")]
        public IActionResult GetAllTables()
        {
            var result = _repository.GetAllTables();
            var msg = new Message();
            if (result.Tables.Count>0)
            {

                msg.IsSuccess = true;
                msg.Data = result;
            }
            else
            {

                msg.IsSuccess = false;
                msg.ReturnMessage = "no values available";
            }


            return Ok(msg);
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var result = _repository.GetAllUsers();
            var msg = new Message();
            if (result.Tables.Count > 0)
            {

                msg.IsSuccess = true;
                msg.Data = result;
            }
            else
            {

                msg.IsSuccess = false;
                msg.ReturnMessage = "no values available";
            }


            return Ok(msg);
        }
        //[HttpGet("GetAllAssets")]
        //public async Task<ActionResult<IEnumerable<AssetModel>>> GetAllAssets([FromQuery]int pageNumber=1,[FromQuery] int pageSize=5)
        //{
        //    var msg = new Message();
        //    var assets = await _repository.GetAllAssets_Paginated(pageNumber, pageSize);
        //    if (assets.Count>0)
        //    {
        //        msg.IsSuccess = true;
        //        msg.Data = assets;
        //    }

        //    else {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "no values found";
        //    }
        //    return Ok(msg);
        //}

        //    [HttpGet]
        //public async Task<ActionResult<IPagedList<AssetModel>>> GetAllAssets(int pageNumber = 1, int pageSize = 10)
        //{
        //    var assets = await _repository.GetAll(pageNumber, pageSize);
        //    return Ok(assets.ToPagedList(pageNumber, pageSize));
        //}

        //[HttpGet("/pages")]

        //    public async Task<ActionResult<List<AssetModel>>> GetAllAssets_Paginated([FromQuery] AssetPages pages)
        //    {
        //        var assets = await _repository.GetAllAssets_Paginated(pages);
        //        return Ok(assets);
        //    }
        //  GET api/values/5
        [HttpGet("Getid/{id}")]
        public async Task<ActionResult<IEnumerable<AssetModel>>> Get(int id)
        {
            var msg = new Message();
            var response = await _repository.GetId(id);
            if (response.Count>0) { 
                msg.IsSuccess = true;
                msg.Data = response;
            }
            else
            {
                msg.IsSuccess =false;
                msg.ReturnMessage="no asset found";
            }
            return Ok(msg);
        }

        // POST api/values
        [HttpGet("GetAllAssets")]
        public async Task<ActionResult<IEnumerable<AssetModel>>> SearchAssets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int Brcid = 0, [FromQuery] int Typeid = 0, [FromQuery] int Vendid = 0,[FromQuery] int Statid=0,[FromQuery] int DateFilter=0,[FromQuery] string StartDate=null, [FromQuery] string EndDate = null)//, [FromQuery] string ptype=null, [FromQuery] string mtype=null, [FromQuery] string rtype =null, [FromQuery] string btype=null)
        {
            var msg = new Message();
            var assets = await _repository.SearchAssets(pageNumber, pageSize, searchTerm, Brcid, Typeid,Vendid,Statid,DateFilter,StartDate,EndDate);//,ptype,mtype,rtype,btype);
            if (assets.Count>0)
            {
                msg.IsSuccess = true;
                msg.Data = assets;
                
                
            }
          
            else {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values found"; 
            }
            return Ok(msg);
        }
        


    




        [HttpPost("AddNew")]
        public async Task<IActionResult> Post([FromBody] AssetModel asset)
        {
          
            var msg = new Message();
            await _repository.Insert(asset);
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
            else{
                msg.IsSuccess = false;
                msg.ReturnMessage = "registeration unscessfull";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] AssetModel asset)
        {
            
            var msg = new Message();
          
           
            //var GetAsset = await _repository.GetById(asset);
            //if (GetAsset.Count>0)
            //{

                await _repository.Update(asset);
                bool success = _repository.IsSuccess;
                //bool exists = _repository.Itexists;
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
            //    msg.ReturnMessage = "no values found"; 
               
            //}

            
            return Ok(msg);
        }
        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(int id, int Branch, string Description)
        {

            var msg = new Message();


            var getasset = await _repository.GetId(id);
            //if (GetAsset.Count>0)
            //{

            await _repository.Transfer(id,Branch,Description);
            bool success = _repository.IsSuccess;
            //bool exists = _repository.Itexists;
            if (success is true)
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = "values transfered successfully";
            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = " transfer unsuccessfull";
            }
            //}
            //else
            //{
            //    msg.IsSuccess = false;
            //    msg.ReturnMessage = "no values found"; 

            //}


            return Ok(msg);
        }
        // DELETE api/values/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id=0)
        {
            var msg = new Message();

            var GetAsset = await _repository.GetId(id);
            if (GetAsset.Count > 0)
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


