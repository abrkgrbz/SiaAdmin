using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.BlockList.CreateBlockList;
using SiaAdmin.Application.Features.Queries.BlockList.GetAllBlockList;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockListController : BaseApiController
    {
        private IMediator _mediator;

        public BlockListController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("GetAllBlockList")]
        public async Task<IActionResult> GetBlockList([FromQuery]GetAllBlockListQueryRequest getAllBlockListQueryRequest)
        {
            var response=await _mediator.Send(getAllBlockListQueryRequest);
            return Ok(response);
        }

        [HttpPost("CreateBlockedUserList")]
        public async Task<IActionResult> CreateBlockList([FromQuery] CreateBlockListRequest createBlockListRequest)
        {
            var response = await _mediator.Send(createBlockListRequest);
            return Ok(response);
        }
    }
}
