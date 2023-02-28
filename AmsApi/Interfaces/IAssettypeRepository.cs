using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmsApi.Interfaces
{
    public interface IAssettypeRepository
    {
        public Task<ActionResult<IEnumerable<AssettypeModel>>> GetAllTypes( int PageNumber = 1,int PageSize = 5);
        public Task<ActionResult<IEnumerable<AssettypeModel>>> SearchTypes(int pageNumber = 1, int pageSize = 5, string searchTerm = null, int typeid = 0);
        public Task<ActionResult<IEnumerable<AssettypeModel>>> Get(int id);
        public Task<IActionResult> Post([FromBody] AssettypeModel type);
        public Task<IActionResult> Update([FromBody] AssettypeModel type, int id = 0);
        public Task<IActionResult> Delete(int id = 0);
    }
}

