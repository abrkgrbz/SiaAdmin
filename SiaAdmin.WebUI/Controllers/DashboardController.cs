using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiaAdmin.Application.Features.Queries.Dashboard.GetPassiveUserStats;
using SiaAdmin.Application.Features.Queries.Dashboard.GetProfileLifetimeStats;
using SiaAdmin.Application.Features.Queries.SiaUser.GetNumberOfUser;
using SiaAdmin.Application.Features.Queries.StoredProcedure.TanitimAnketiDolduran;
using SiaAdmin.Application.Features.Queries.StoredProcedure.ToplamAnketBilgisi;
using SiaAdmin.Application.Features.Queries.User.GetListLastSeenAdet;
using SiaAdmin.Application.Features.Queries.User.GetListLastSeenSaat;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            var response = await Mediator.Send(getNumberOfFillingSurveyIntroRequest);
            return Ok(response);
        }


        [HttpGet("dashboard/son-gorulen-adet")] 
        public async Task<IActionResult> GetListLastSeenAdet( )
        {
            var response = await Mediator.Send(new GetListLastSeenAdetRequest());
            return Ok(response);
        }

        [HttpGet("dashboard/son-gorulen-saat")] 
        public async Task<IActionResult> GetListSeenSaat( )
        {
            var response = await Mediator.Send(new GetListLastSeenSaatRequest());
            return Ok(response);
        }

        [HttpGet("profile-lifetime")]
        public async Task<IActionResult> GetProfileLifetimeStats(GetProfileLifetimeStatsRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok(new{data= response });
        }

        [HttpGet("passive-users")]
        public async Task<IActionResult> GetPassiveUserStats(GetPassiveUserStatsRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok(new { data = response });
        }
    }
}
