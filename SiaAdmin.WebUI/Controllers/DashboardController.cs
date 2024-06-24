using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.SiaUser.GetNumberOfUser;
using SiaAdmin.Application.Features.Queries.StoredProcedure.TanitimAnketiDolduran;
using SiaAdmin.Application.Features.Queries.StoredProcedure.ToplamAnketBilgisi;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {

        [HttpGet("dashboard/toplam-anket-bilgisi")]
        public async Task<IActionResult> GetTotalSurveyInformation(GetTotalSurveyInformationRequest getTotalSurveyInformationRequest)
        {
            getTotalSurveyInformationRequest.EndDate = null;
            getTotalSurveyInformationRequest.StartDate = null;
            var response = await Mediator.Send(getTotalSurveyInformationRequest);
            return Ok(response);
        }

        [HttpGet("dashboard/panelist-sayisi")]
        public async Task<IActionResult> GetNumberOfUser(GetNumberOfUserRequest getNumberOfUserRequest)
        {
            getNumberOfUserRequest.isDistinct = false;
            var response = await Mediator.Send(getNumberOfUserRequest);
            return Ok(response);
        }

        [HttpGet("dashboard/tanitim-anketi-dolduran-sayisi")]
        public async Task<IActionResult> GetNumberOfFillingSurveyIntro(
            GetNumberOfFillingIntroRequest getNumberOfFillingSurveyIntroRequest)
        {
            getNumberOfFillingSurveyIntroRequest.EndDate = null;
            getNumberOfFillingSurveyIntroRequest.StartDate = null;
            var response = await Mediator.Send(getNumberOfFillingSurveyIntroRequest);
            return Ok(response);
        }
    }
}
