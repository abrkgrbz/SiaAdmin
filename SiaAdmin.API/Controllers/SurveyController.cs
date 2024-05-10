using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Features.Queries.BlockList.GetAllBlockList;
using SiaAdmin.Application.Features.Queries.Survey.GetAllSurvey;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SurveyController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetAllSurveyList")]
        public async Task<IActionResult> GetSurveyList([FromQuery]GetAllSurveyQueryRequest getAllSurveyQueryRequest)
        {
            var response = await _mediator.Send(getAllSurveyQueryRequest);
            return Ok(response);
        }

        [HttpPost("CreateSurvey")]
        public async Task<IActionResult> CreateSurvey([FromQuery] CreateSurveyRequest createSurveyRequest)
        {
            var response = _mediator.Send(createSurveyRequest);
            return Ok(response);
        }
    }
}
