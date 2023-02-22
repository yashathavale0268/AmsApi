using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using AmsApi.Repository;
using CoreApiAdoDemo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapController : ControllerBase
    {
        private readonly ScrapRepository _repository;
        public ScrapController(ScrapRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<ScrapModel>>> GetAllScrap([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 5)
        {
            var msg = new Message();
            var scrap = await _repository.GetAllScrap(PageNumber, PageSize);
            if (scrap == null) { return NotFound(); }
            return scrap;
        }
        //public async Task<ActionResult<IEnumerable<ScrapModel>>> Get()
        //{
        //    return await _repository.GetAllScraps();
        //}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<ScrapModel>>> SearchScrap([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = null, [FromQuery] int searchId = 0, [FromQuery] int assetId = 0, [FromQuery] int brcId = 0, [FromQuery] int vedId = 0, [FromQuery] int empId = 0)
        {
            var msg = new Message();
            var Scrap = await _repository.SearchScrap(pageNumber, pageSize, searchTerm, searchId, assetId, brcId, vedId, empId) ;
            if (Scrap == null) { return NotFound(); }
            return Scrap;
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScrapModel>> Get(int id)
        {
            var response = await _repository.GetScrapId(id);
            if (response == null) { return NotFound(); }
            return response;
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] ScrapModel request)
        {
            var msg = new Message();
            await _repository.Insert(request);
        }

        // PUT api/values/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScrapModel request)
        {
            var msg = new Message();
            var GetScrap = await _repository.GetScrapId(id);
            if (GetScrap == null)
            {
                return NotFound();
            }

            await _repository.UpdateScrap(request, id);

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("Delete/{id}")]
        public async Task Delete(int id)
        {
            var msg = new Message();
            await _repository.DeleteById(id);
        }
    }
}

