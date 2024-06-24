using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.User.CreateUser;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [NonController]
    public class UserController : BaseApiController
    {
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromQuery]CreateUserRequest createUserRequest)
        {
            var response = Mediator.Send(createUserRequest);
            return Ok(response);
        }
    }
}
