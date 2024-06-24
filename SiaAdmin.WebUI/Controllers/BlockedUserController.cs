using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.BlockList.GetBlockedListDataTable;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class BlockedUserController : BaseController
    {
        [HttpGet("blocklu-kullanici-listesi")]
        public IActionResult BlockedList()
        {
            return View();
        }

        [HttpPost("blocklu-kullanici-listesi/LoadTable")]
        public async Task<IActionResult> GetBlockedList(GetBlockedListDataTableRequest getBlockedListDataTableRequest)
        {
            getBlockedListDataTableRequest.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getBlockedListDataTableRequest.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getBlockedListDataTableRequest.orderColumnName = Request.Form[$"columns[{getBlockedListDataTableRequest.orderColumnIndex}][name]"].FirstOrDefault();
            getBlockedListDataTableRequest.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response =await Mediator.Send(getBlockedListDataTableRequest);
            return Ok(response);
        }

        [HttpGet("blocklu-kullanici-yukleme")]
        public IActionResult UploadBlockedUserView()
        {
            return View();
        }
    }
}
