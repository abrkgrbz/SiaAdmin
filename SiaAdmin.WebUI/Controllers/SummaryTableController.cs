using System.Text;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.DataTableExcel;
using SiaAdmin.Application.Features.Queries.DataTableExcel.ChurnDataExport;
using SiaAdmin.Application.Features.Queries.EODPTable;
using SiaAdmin.Application.Features.Queries.EODTable;
using SiaAdmin.Application.Features.Queries.EODTable.GetAllEODTable;
using SiaAdmin.Application.Features.Queries.User.GetAllChurnDataList;
using SiaAdmin.Application.Features.Queries.User.GetChurnDataList;
using SiaAdmin.Application.Features.Queries.WaitData.GetDataTablesWaitData;
using SiaAdmin.Domain.Entities.Models;

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
        [HttpGet("churn-data")]
        public async Task<IActionResult> ChurnData()
        { 
            return View();
        }

        [HttpPost("churn-data/LoadTable")]
        public async Task<IActionResult> ChurnDataLoadTable(GetChurnDataListRequest getChurnDataListRequest)
        {

            getChurnDataListRequest.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getChurnDataListRequest.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getChurnDataListRequest.orderColumnName = Request.Form[$"columns[{getChurnDataListRequest.orderColumnIndex}][name]"].FirstOrDefault();
            getChurnDataListRequest.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getChurnDataListRequest);
            return Ok(response);
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

		[HttpPost("eod-table/ExportExcel")]
		public async Task<IActionResult> ExportExcelEOD()
		{
			var list = await Mediator.Send(new GetAllEODTableRequest());
			var response = await Mediator.Send(new DataTableExportExcelRequest<EODTableViewModel>() { data = list.EodTableViewModels}); 
			return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EODData.xlsx");
		}
        [HttpPost("churn-data/ExportExcel")]
        public async Task<IActionResult> ExportChurnData()
        { 
            var response = await Mediator.Send(new ChurnDataExportRequest());
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ChurnData.xlsx");
        }
		
    }
}
