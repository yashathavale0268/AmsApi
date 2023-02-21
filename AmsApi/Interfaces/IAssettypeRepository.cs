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
        Task<ActionResult<IEnumerable<AssettypeModel>>> GetAllTypes(int PageNumber, int PageSize);
        Task<ActionResult<IEnumerable<AssettypeModel>>> SearchTypes(int pageNumber = 1, int pageSize = 5, string searchTerm = null, int typeid = 0);
        Task<AssettypeModel> GettypeById(int id);
        Task Insert(AssettypeModel type);
        Task UpdateType(AssettypeModel branch, int id);
        Task DeletetypeById(int id);
    }
}

