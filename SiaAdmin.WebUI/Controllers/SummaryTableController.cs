using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.EODPTable;
using SiaAdmin.Application.Features.Queries.EODTable;
using SiaAdmin.Application.Features.Queries.WaitData.GetDataTablesWaitData;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class SummaryTableController : BaseController
    {
        [HttpGet("eod-table")]
        public IActionResult EODTable()
        {
            return View();
        }
        [HttpGet("eodp-table")]
        public IActionResult EODPTable()
        {
            return View();
        }



        [HttpPost("eod-table/LoadTable")]
        public async Task<IActionResult> EODLoadTable(EODTableQueryRequest getEodTableQueryRequest)
        {
            getEodTableQueryRequest.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getEodTableQueryRequest.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getEodTableQueryRequest.orderColumnName = Request.Form[$"columns[{getEodTableQueryRequest.orderColumnIndex}][name]"].FirstOrDefault();
            getEodTableQueryRequest.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getEodTableQueryRequest);
            return Ok(response);
        }

        [HttpPost("eodp-table/LoadTable")]
        public async Task<IActionResult> EODPLoadTable(EODPTableQueryRequest getEodpTableQueryRequest)
        {
            getEodpTableQueryRequest.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getEodpTableQueryRequest.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getEodpTableQueryRequest.orderColumnName = Request.Form[$"columns[{getEodpTableQueryRequest.orderColumnIndex}][name]"].FirstOrDefault();
            getEodpTableQueryRequest.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getEodpTableQueryRequest);
            return Ok(response);
        }


    }
}
