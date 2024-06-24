using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.Point.GetPointListByInternalGuid;
using SiaAdmin.Application.Features.Queries.Point.GetPointListBySurveyId;
using SiaAdmin.Application.Features.Queries.Survey.GetSelectListItemSurvey;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class PointController : BaseController
    {

        [HttpGet("puan-kontrol")]
        public IActionResult PointCheck()
        {
            return View();
        }
        [HttpGet("proje-bazli-puan-kontrol")]
        public async Task<IActionResult> PointCheckBySurvey(GetSelectListItemSurveyRequest getSelectListItemRequest)
        {
            var data = await Mediator.Send(getSelectListItemRequest);
            ViewBag.SurveyList = data.SurveySelectListItems;
            return View();
        }

        [HttpGet("puan-yukle")]
        public IActionResult PointAdd()
        {
            return View();
        }

        [HttpPost("puan-yukle/AddPoint")]
        public async Task<IActionResult> AddPoint()
        {
            return Ok();
        }

        [HttpPost("puan-kontrol/CheckPoint")]
        public async Task<IActionResult> CheckPoint(GetPointListByInternalGuidRequest getPointListByInternalGuidRequest)
        { 
            var response = await Mediator.Send(getPointListByInternalGuidRequest);
            return Ok(response);
        }
        [HttpPost("puan-kontrol/CheckPointBySurveyId")]
        public async Task<IActionResult> CheckPointBySurvey(GetPointListBySurveyIdRequest getPointListBySurveyIdRequest)
        {
            var response = await Mediator.Send(getPointListBySurveyIdRequest);
            return Ok(response);
        }
    }
}
