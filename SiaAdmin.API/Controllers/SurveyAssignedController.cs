using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.SurveyAssigned.CreateSurveyAssigned;
using SiaAdmin.Application.Features.Queries.SurveyAssigned.GetAllSurveyAssigned;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [NonController]
    public class SurveyAssignedController : ControllerBase
    {
        private IMediator _mediator;

        public SurveyAssignedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSurveyAssigned([FromQuery]GetAllSurveyAssignedQueryRequest getAllSurveyAssignedQueryRequest)
        {
            var response = await _mediator.Send(getAllSurveyAssignedQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurveyAssigned([FromQuery] CreateSurveyAssignedRequest createSurveyAssignedRequest)
        {
            var response = await _mediator.Send(createSurveyAssignedRequest);
            return Ok(response);
        }
    }
}
