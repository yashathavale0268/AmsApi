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
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AmsApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetModel>>> GetAllAssets([FromQuery]int pageNumber=1,[FromQuery] int pageSize=5)
        {
            var msg = new Message();
            var assets = await _repository.GetAllAssets_Paginated(pageNumber, pageSize);
            if (assets == null)
            {
                msg.ReturnMessage = "no values found";

            }
            else {
                msg.IsSuccess = true;
                msg.Data= assets; 
            }
            return Ok(msg);
        }

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
        // GET api/values/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<AssetModel>> Get(int id)
        //{
        //    var response = await _repository.GetById(id);
        //    if (response == null) { return NotFound(); }
        //    return response;
        //}

        // POST api/values
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AssetModel>>> SearchAssets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int Brcid =0 , [FromQuery] int Typeid=0, [FromQuery] int Empid=0)//, [FromQuery] string ptype=null, [FromQuery] string mtype=null, [FromQuery] string rtype =null, [FromQuery] string btype=null)
        {
            var msg = new Message();
            var assets = await _repository.SearchAssets(pageNumber, pageSize, searchTerm, Brcid, Typeid, Empid);//,ptype,mtype,rtype,btype);
            if (assets.Count>0)
            {
                return assets;
                
                
            }
            else { msg.ReturnMessage = "no values found"; }
            return Ok(msg);
        }
        


    




        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AssetModel asset)
        {
            var msg = new Message();
            await _repository.Insert(asset);
            bool exists = _repository.Itexists;
            bool success = _repository.IsSuccess;
            
            if (exists is true)
            {
                msg.ReturnMessage = "Item alredy registered";
            }
            else if (success is true)
                {
                    msg.ReturnMessage = " new entry succesfully registered";
                }
            else{
                msg.ReturnMessage = "registeration unscessfull";
            }
            return Ok(msg);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] AssetModel asset, int id=0)
        {
            var msg = new Message();
          
           
            var GetAsset = await _repository.GetById(id);
            if (GetAsset.Count>0)
            {

                await _repository.Update(asset, id);
                bool success = _repository.IsSuccess;
                //bool exists = _repository.Itexists;
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

                msg.ReturnMessage = "no values found"; 
               
            }

            
            return Ok(msg);
        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromQuery] int id = 0)
        {
            var msg = new Message();

            var GetAsset = await _repository.GetById(id);
            if (GetAsset.Count > 0)
            {
                await _repository.DeleteById(id);
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


