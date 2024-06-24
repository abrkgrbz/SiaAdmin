using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.FilterData.GetDataTableFilterData;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [NonController]
    public class FilterDataController : ControllerBase
    {
        private IMediator _mediator;

        public FilterDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetFilterData")]
        public async Task<IActionResult> GetFilterDataList([FromQuery]
            GetDataTableFilterDataQueryRequest dataTableFilterDataQueryRequest)
        {
            var response = await _mediator.Send(dataTableFilterDataQueryRequest);
            return Ok(response);
        }
    }
}
