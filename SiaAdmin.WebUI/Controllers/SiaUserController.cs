using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.SiaUser.GetAllSiaUser;
using SiaAdmin.Application.Features.Queries.SiaUser.GetByGuidSiaUser;
using SiaAdmin.Application.Features.Queries.SurveyLog.GetUserLogDetail;
using SiaAdmin.Application.Features.Queries.User.GetUserList;
using SiaAdmin.WebUI.Models;

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

        [Route("/adm/panelist-profil")]
        [HttpPost]
        public async Task<IActionResult> SiaUserProfile([FromForm] string guid)
        {
            SiaUserProfileViewModel model = new();
            GetByGuidSiaUserRequest getByGuidSiaUserRequest = new GetByGuidSiaUserRequest();
            getByGuidSiaUserRequest.SurveyUserGuid=Guid.Parse(guid);
            var response =await Mediator.Send(getByGuidSiaUserRequest);
            model.Active = response.GetUserByGuidViewModel.Active;
            model.Email=response.GetUserByGuidViewModel.Email;
            model.LastIP=response.GetUserByGuidViewModel.LastIP;
            model.LastLogin=response.GetUserByGuidViewModel.LastLogin;
            model.LoginCount=response.GetUserByGuidViewModel.LoginCount;
            model.Msisdn = response.GetUserByGuidViewModel.Msisdn;
            model.Name=response.GetUserByGuidViewModel.Name;
            model.ProfilPuani = response.GetUserByGuidViewModel.ProfilPuani;
            model.RegistrationDate=response.GetUserByGuidViewModel.RegistrationDate;
            model.Surname=response.GetUserByGuidViewModel.Surname;
            return View(model);
        }

        public async Task<IActionResult> SiaUserProfileDetails(GetUserLogdDetailRequest getUserLogdDetailRequest)
        {

            getUserLogdDetailRequest.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getUserLogdDetailRequest.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getUserLogdDetailRequest.orderColumnName = Request.Form[$"columns[{getUserLogdDetailRequest.orderColumnIndex}][name]"].FirstOrDefault();
            getUserLogdDetailRequest.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getUserLogdDetailRequest);
            return Ok(response);
        }
     

    }
}
