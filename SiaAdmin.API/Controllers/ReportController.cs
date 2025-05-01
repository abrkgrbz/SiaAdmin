using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using SiaAdmin.Application.Features.Queries.SiaUser.GetNumberOfUser;
using SiaAdmin.Application.Features.Queries.StoredProcedure.ToplamAnketBilgisi;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [NonController]
    public class ReportController : BaseApiController
    {
        [HttpGet("GetNumberOfPanelist")]
        public async Task<IActionResult> GetNumberOfUser([FromQuery] GetNumberOfUserRequest getNumberOfUserRequest)
        {
            var response = await Mediator.Send(getNumberOfUserRequest);
            return Ok(response);
        }

        [HttpGet("GetTotalSurveyInformation")]
        public async Task<IActionResult> GetTotalSurveyInformation([FromQuery] GetTotalSurveyInformationRequest getTotalSurveyInformationRequest)
        {
            var response = await Mediator.Send(getTotalSurveyInformationRequest);
            return Ok(response);
        }
    }
}
