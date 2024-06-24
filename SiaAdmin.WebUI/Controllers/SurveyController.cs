using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class SurveyController : BaseController
    {
        

        [HttpGet("proje-listesi")]
        public IActionResult List()
        {
            return View();
        } 

        [HttpPost("proje-listesi/proje-ekle")]
        public async Task<IActionResult> SurveyAdd(CreateSurveyRequest createSurveyRequest)
        {
            var response = await Mediator.Send(createSurveyRequest);
            return Ok(response);
        }

        [HttpPost("proje-listesi/LoadTable")]
        public async Task<IActionResult> LoadTable(GetDataTableSurveyQueryRequest getDataTableSurvey)
        {
            getDataTableSurvey.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getDataTableSurvey.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getDataTableSurvey.orderColumnName = Request.Form[$"columns[{getDataTableSurvey.orderColumnIndex}][name]"].FirstOrDefault();
            getDataTableSurvey.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getDataTableSurvey);
            return Ok(response);
        }

        [Authorize("AdminOnly")]
        [HttpPost("proje-kapat")]
        public async Task<IActionResult> CloseProject()
        {
            return Ok();
        }
    }
}
