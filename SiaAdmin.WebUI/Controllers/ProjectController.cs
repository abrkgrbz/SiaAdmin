using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.FilterData.GetDataTableFilterData;
using SiaAdmin.Application.Features.Queries.WaitData.GetDataTablesWaitData;
using SiaAdmin.Application.Features.Queries.WaitData.GetSummaryWaitData;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        [HttpGet("bekleyen-proje-listesi")]
        public IActionResult WaitDataList()
        {
            return View();
        }

        [HttpGet("filterdata-listesi")]
        public IActionResult FilterDataList()
        {
            return View();
        }

        [HttpGet("kapali-proje-listesi")]
        public IActionResult ClosedProjectList()
        {
            return View();
        }

        [HttpPost("filterdata-listesi/LoadTable")]
        public async Task<IActionResult> LoadTableFilterData(GetDataTableFilterDataQueryRequest getDataTableFilterDataQueryRequest)
        {
            getDataTableFilterDataQueryRequest.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getDataTableFilterDataQueryRequest.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getDataTableFilterDataQueryRequest.orderColumnName = Request.Form[$"columns[{getDataTableFilterDataQueryRequest.orderColumnIndex}][name]"].FirstOrDefault();
            getDataTableFilterDataQueryRequest.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response =await Mediator.Send(getDataTableFilterDataQueryRequest);
            return Ok(response);
        }

        [HttpPost("bekleyen-proje-listesi/LoadTable")]
        public async Task<IActionResult> LoadTableWaitData(GetSummaryWaitDataRequest getSummaryWaitDataRequest)
        {
            getSummaryWaitDataRequest.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getSummaryWaitDataRequest.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getSummaryWaitDataRequest.orderColumnName = Request.Form[$"columns[0][name]"].FirstOrDefault();
            getSummaryWaitDataRequest.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getSummaryWaitDataRequest);
            return Ok(response);
        }

     
    }
}
