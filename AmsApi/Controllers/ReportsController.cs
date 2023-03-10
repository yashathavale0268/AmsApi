using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        //private readonly LoginRepository _repository;
        ////  private readonly AmsRepository _common;
        ////  private readonly string key;
        //public RegisterationController(LoginRepository repository, /*AmsRepository common,*/ ILogger<RegisterationController> logger)
        //{
        //    this._repository = repository ?? throw new ArgumentNullException(nameof(repository));

        //    //  this._common = common ?? throw new ArgumentNullException(nameof(common));
        //}


        //[HttpGet]
        //[Route("GetAllUsers")]
        //public async Task<ActionResult<IEnumerable<UserModel>>> GetAll([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)  //
        //{                                                    //GetAllUser(int pageNumber, int pageSize)
        //    var msg = new Message();                         //GetAllUser(PageNumber, PageSize);
        //    var Users = await _repository.GetAllUser(PageNumber, PageSize);
        //    if (Users.Count > 0)
        //    {

        //        msg.IsSuccess = true;
        //        msg.Data = Users;

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
