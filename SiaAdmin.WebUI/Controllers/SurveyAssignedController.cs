using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiaAdmin.Application.Features.Commands.SurveyAssigned.CreateSurveyAssigned;
using SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey;
using SiaAdmin.Application.Features.Queries.Survey.GetSelectListItemSurvey;
using SiaAdmin.Application.Features.Queries.SurveyAssigned.GetCountSurveyAssigned;
using SiaAdmin.Application.Features.Queries.SurveyAssigned.GetDuplicatedRecord;
using SiaAdmin.Application.Features.Queries.WaitData.GetDataTablesWaitData;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class SurveyAssignedController : BaseController
    {
         
        [HttpGet("proje-atama-listesi")]
        public async Task<IActionResult> List(GetSelectListItemSurveyRequest getSelectListItemRequest)
        {
            var data= await Mediator.Send(getSelectListItemRequest);
            ViewBag.SurveyList = data.SurveySelectListItems;
            return View();
             
           
        }

        [HttpPost("proje-atama-listesi/proje-ata")]
        public async Task<IActionResult> SurveyAssignedAdd(CreateSurveyAssignedRequest createSurveyAssignedRequest)
        {
            var response =await Mediator.Send(createSurveyAssignedRequest);
            return Ok(response);
        }

        [HttpPost("proje-atama-listesi/LoadTable")]
        public async Task<IActionResult> LoadTable(GetDataTablesWaitDataRequest getDataTablesWaitDataRequest)
        {
            getDataTablesWaitDataRequest.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getDataTablesWaitDataRequest.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getDataTablesWaitDataRequest.orderColumnName = Request.Form[$"columns[{getDataTablesWaitDataRequest.orderColumnIndex}][name]"].FirstOrDefault();
            getDataTablesWaitDataRequest.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getDataTablesWaitDataRequest);
            return Ok(response);
        }

        [HttpGet("mukerrer-kayıt-sorgulama-noktasi")]
        public async Task<IActionResult> CheckDuplicatedRecord(GetSelectListItemSurveyRequest getSelectListItemRequest)
        {
            var data = await Mediator.Send(getSelectListItemRequest);
            ViewBag.SurveyList = data.SurveySelectListItems;
            return View();
        }

        [HttpPost("check-duplicated-record")]
        public async Task<IActionResult> CheckDuplicateRecordWithSurveyId(GetDuplicatedRecordRequest getDuplicatedRecordRequest)
        {
            var response = await Mediator.Send(getDuplicatedRecordRequest);
            return Ok(response);
        }

        [HttpPost("count-survey-assigned")]
        public async Task<IActionResult> CountSurveyAssignedWithSurveyId(GetCountSurveyAssignedRequest getCountSurveyAssignedRequest)
        {
            var response = await Mediator.Send(getCountSurveyAssignedRequest);
            return Ok(response);
        }
    }
}
