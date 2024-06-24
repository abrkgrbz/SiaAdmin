using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLog;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [NonController]
    public class SurveyLogController : ControllerBase
    {
        private IMediator _mediator;

        public SurveyLogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreatePoint")]
        public async Task<IActionResult> AddSurveyLog([FromQuery] CreateSurveyLogRequest createSurveyLogRequest)
        {
            var reponse = await _mediator.Send(createSurveyLogRequest);
            return Ok(reponse);
        }

    }
}
