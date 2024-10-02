using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.OTPHistory.CreateOTPHistory;
using SiaAdmin.Application.Features.Queries.OTPHistory.GetOTPHistory;
using SiaAdmin.Application.Features.Queries.OTPHistory.VerifyOTPHistory;
using SiaAdmin.Application.Features.Queries.SiaUser.GetByPhoneNumberUser;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [NonController]
    public class OTPHistoryController : BaseApiController
    {
        

        [HttpPost("CreateOTP")]
        public async Task<IActionResult> CreateOTP([FromQuery] CreateOTPHistoryRequest createOtpHistoryRequest)
        {
            var response = await Mediator.Send(createOtpHistoryRequest);
            return Ok(response);
        }
        [HttpGet("GetOTPWithPhoneNumber")]
        public async Task<IActionResult> GetOTP([FromQuery]GetOTPHistoryRequest getOtpHistoryRequest)
        {
            var response = await Mediator.Send(getOtpHistoryRequest);
            return Ok(response);
        }
        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromQuery] VerifyOTPHistoryRequest verifyOtpHistoryRequest)
        {
            var response = await Mediator.Send(verifyOtpHistoryRequest);
            return Ok(response);
        }

  

    }
}
