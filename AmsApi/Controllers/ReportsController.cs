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
        [HttpGet]
        [Route("BranchReport")]
        public async Task<ActionResult> GetBranchReport([FromQuery] int brch=0)  //, [FromQuery], [FromQuery], [FromQuery]
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var AssetReport = await _repository.GetBranchviseReport(brch);
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
        [HttpGet]
        [Route("CompanyReport")]
        public async Task<ActionResult> GetCompanyReport([FromQuery] int comp = 0)  //, [FromQuery], [FromQuery], [FromQuery]
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var AssetReport = await _repository.GetCompanyviseReport(comp);
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
        [HttpGet]
        [Route("DepartmentReport")]
        public async Task<ActionResult> GetDepartmentReport([FromQuery] int dep = 0)  //, [FromQuery], [FromQuery], [FromQuery]
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var AssetReport = await _repository.GetDepartmentviseReport(dep);
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
