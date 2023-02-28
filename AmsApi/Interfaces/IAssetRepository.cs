using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using Microsoft.AspNetCore.Mvc;
namespace AmsApi.Interfaces
{
    public interface IAssetRepository
    {
        public Task<ActionResult<IEnumerable<AssetModel>>> GetAllAssets( int pageNumber = 1, int pageSize = 5);
        public Task<ActionResult<IEnumerable<AssetModel>>> SearchAssets( int pageNumber = 1, int pageSize = 5, string searchTerm = null,  int Brcid = 0, int Typeid = 0, int Empid = 0);
        public Task<ActionResult<IEnumerable<AssetModel>>> Get(int id);
        public Task<IActionResult> Post([FromBody] AssetModel asset);
        public Task<IActionResult> Update(AssetModel asset, int id = 0);
        public Task<IActionResult> Delete(int id = 0);
    }
}
