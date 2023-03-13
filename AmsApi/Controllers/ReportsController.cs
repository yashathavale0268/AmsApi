using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Repository;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ReportsRepository _repository;
        //  private readonly AmsRepository _common;
        //  private readonly string key;
        public ReportsController(ReportsRepository repository, /*AmsRepository common,*/ ILogger<ReportsController> logger)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));

            //  this._common = common ?? throw new ArgumentNullException(nameof(common));
        }


        [HttpGet]
        [Route("AssetReport")]
        public async Task<ActionResult> GetAssetReport([FromQuery] int typ=0)  //
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var AssetReport = await _repository.GetAssetReport(typ);
            if (AssetReport is not null)
            {

                msg.IsSuccess = true;
                msg.Data = AssetReport;

            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no user found";
            }
            return Ok(msg);
        }
        //[HttpGet]
        //[Route("AssetReport")]
        //public async Task<ActionResult> GetAssetReport([FromQuery] , [FromQuery], [FromQuery], [FromQuery])  //
        //{                                                    //GetAllUser(int pageNumber, int pageSize)
        //    var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
        //    var AssetReport = await _repository.GetAssetReport();
        //    if (AssetReport is not null)
        //    {

        //        msg.IsSuccess = true;
        //        msg.Data = AssetReport;

        //    }
        //    else
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "no user found";
        //    }
        //    return Ok(msg);
        //}
        //[HttpGet]
        //[Route("AssetReport")]
        //public async Task<ActionResult> GetAssetReport()  //
        //{                                                    //GetAllUser(int pageNumber, int pageSize)
        //    var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
        //    var AssetReport = await _repository.GetAssetReport();
        //    if (AssetReport is not null)
        //    {

        //        msg.IsSuccess = true;
        //        msg.Data = AssetReport;

        //    }
        //    else
        //    {
        //        msg.IsSuccess = false;
        //        msg.ReturnMessage = "no user found";
        //    }
        //    return Ok(msg);
        //}
    }
}
