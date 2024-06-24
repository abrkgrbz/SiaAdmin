using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.SiaUser.GetAllSiaUser;
using SiaAdmin.Application.Features.Queries.SiaUser.GetByGuidSiaUser;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class SiaUserController : BaseController
    {

        [HttpGet("sia-kullanici-tablosu")]
        public IActionResult UserList()
        {
            return View();
        }

        [HttpPost("sia-kullanici-tablosu/LoadTable")]
        public async Task<IActionResult> LoadTable(GetAllSiaUserRequest getAllSiaUserQuery)
        {
            getAllSiaUserQuery.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getAllSiaUserQuery.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getAllSiaUserQuery.orderColumnName = Request.Form[$"columns[{getAllSiaUserQuery.orderColumnIndex}][name]"].FirstOrDefault();
            getAllSiaUserQuery.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getAllSiaUserQuery);
            return Ok(response);
        }

        [HttpGet("panelist-profili/{guid}")]
        public async Task<IActionResult> SiaUserProfile(string guid)
        {
            GetByGuidSiaUserRequest getByGuidSiaUserRequest = new GetByGuidSiaUserRequest();
            getByGuidSiaUserRequest.SurveyUserGuid=Guid.Parse(guid);
            var response =await Mediator.Send(getByGuidSiaUserRequest);
            return View(response.data);
        }

    }
}
