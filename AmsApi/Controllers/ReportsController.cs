using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
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
        //Get Dashbord values
        [HttpGet]
        [Route("DashbordValues")]
        public async Task<ActionResult> GetDashbordValues()  //
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var DashbordValues = await _repository.GetDashbordValues();
            if (DashbordValues is not null)
            {

                msg.IsSuccess = true;
                msg.Data = DashbordValues;

            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no values found";
            }
            return Ok(msg);
        }

        // report for assets
        [HttpGet]
        [Route("GetAllAssetReport")]
        public async Task<ActionResult> GetAllAssetReport( [FromQuery] int brch=0, [FromQuery] int typ =0)  //
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var AssetReport = await _repository.GetAssetReport(brch,typ);
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
        //reports for branches
        [HttpGet]
        [Route("BranchReport")]
        public async Task<ActionResult> GetBranchReport([FromQuery] int brch=0,[FromQuery] int typ = 0)  //, [FromQuery], [FromQuery], [FromQuery]
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var AssetReport = await _repository.GetBranchviseReport(brch,typ);
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

        //report for companies
        [HttpGet]
        [Route("CompanyReport")]
        public async Task<ActionResult> GetCompanyReport([FromQuery] int comp = 0, [FromQuery] int typ = 0)  //, [FromQuery], [FromQuery], [FromQuery]
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var CompReport = await _repository.GetCompanyviseReport(comp,typ);
            if (CompReport is not null)
            {

                msg.IsSuccess = true;
                msg.Data = CompReport;

            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no user found";
            }
            return Ok(msg);
        }
        // reports for departments
        [HttpGet]
        [Route("DepartmentReport")]
        public async Task<ActionResult> GetDepartmentReport([FromQuery] int dep = 0, [FromQuery] int typ = 0)  //, [FromQuery], [FromQuery], [FromQuery]
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var DepReport = await _repository.GetDepartmentviseReport(dep,typ);
            if (DepReport is not null)
            {

                msg.IsSuccess = true;
                msg.Data = DepReport;

            }
            else
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "no user found";
            }
            return Ok(msg);
        }

        [HttpGet("ReportGetAllTables")]
        public IActionResult GetAllTables()
            
        {
            var msg = new Message();
            var result = _repository.GetReportTable();

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
        [HttpGet("GetInUseTable")]
        public async Task<ActionResult<IEnumerable<ReportModel>>> GetInUseTables(int pageNumber, int pageSize, string searchString, int brcid, int typ)//

        {
            var msg = new Message();
            var result = await _repository.GetInUseTable(pageNumber, pageSize, searchString, brcid,typ);//

            if (result.Count > 0)
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

        [HttpGet("GetIsSpareTable")]
        public async Task<ActionResult<IEnumerable<ReportModel>>> GetIsSpareTable(int pageNumber, int pageSize, string searchString, int brcid, int typ)

        {
            var msg = new Message();
            var result = await _repository.GetIsSpareTable(pageNumber, pageSize, searchString, brcid,typ);

            if (result.Count > 0)
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
        [HttpGet("GetIsWorkingTable")]
        public async Task<ActionResult<IEnumerable<ReportModel>>> GetIsWorkingTable(int pageNumber, int pageSize, string searchString, int brcid, int typ, int stat)

        {
            var msg = new Message();
            var result = await _repository.GetIsWorkingTable(pageNumber, pageSize, searchString, brcid,typ,stat);

            if (result.Count > 0)
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
        [HttpGet("GetNew_RequestTable")] 
        public async Task<ActionResult<IEnumerable<ReportModel>>> GetNew_RequestsTable(int pageNumber, int pageSize, string searchString, int brcid,int typ)

        {
            var msg = new Message();
            var result = await _repository.GetNew_RequestTable(pageNumber, pageSize, searchString, brcid,typ);

            if (result.Count > 0)
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
        [HttpGet]
        [Route("Qty")]
        public async Task<ActionResult> GetAssetReport()  //
        {                                                    //GetAllUser(int pageNumber, int pageSize)
            var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
            var AssetReport = await _repository.GetQty();
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

    }
}
