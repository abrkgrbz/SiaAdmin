using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.Survey.CloseSurvey;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey;
using SiaAdmin.Application.Features.Queries.Survey.GetLastSurveyId;
 

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    
    public class SurveyController : BaseController
    {
        

        [HttpGet("proje-listesi")]
        public IActionResult List()
        {
            ViewBag.LastSurveyId = GetLastSurveyId().Result;
            return View();
        } 

        [HttpPost("proje-listesi/proje-ekle")]
        public async Task<IActionResult> SurveyAdd(CreateSurveyRequest createSurveyRequest)
        {
            createSurveyRequest.UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
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

        private async Task<int> GetLastSurveyId()
        {
            var response =await Mediator.Send(new GetLastSurveyIdRequest());
            return response.SurveyId;
        }


        [Authorize("AdminOnly")]
        [HttpPost("proje-kapat")]
        public async Task<IActionResult> CloseProject(CloseSurveyRequest request)
         {
            var response=await Mediator.Send(request);
            return Ok(response);
            return Forbid();
        }

     
    }
}
