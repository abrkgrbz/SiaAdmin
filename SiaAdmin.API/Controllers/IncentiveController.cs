using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.SurveyLog.CreateIncentive;
using SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLogByUser;
using SiaAdmin.Application.Features.Queries.Incentive.GetAllIncentice;
 
namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncentiveController : BaseApiController
    {
        /// <summary>
        /// Hediye Listesini döndürür.
        /// </summary>
        [HttpGet("IncentiveList")]
        public async Task<IActionResult> GetIncentiveList([FromQuery]GetAllIncenticeRequest getAllIncentiveRequest)
        {
            var response =await Mediator.Send(getAllIncentiveRequest);
            return Ok(response);
        }
        /// <summary>
        /// Kullanıcı hediye isteği
        /// Token bilgisi gerekir
        /// </summary>
        [HttpGet("RequestIncentive")]
        [Authorize]
        public async Task<IActionResult> RequestIncentivesByUser([FromQuery] RequestIncentive incentive)
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new CreateSurveyLogByUserRequest()
                { InternalGuid = Guid.Parse(userGuid), IncentiveId = incentive.IncentiveID });
            return Ok();
        }

        public class RequestIncentive
        {
            public int IncentiveID { get; set; }
        }
    }
}
