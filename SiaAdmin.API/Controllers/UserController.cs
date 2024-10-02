using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.DeviceRegistrations.CreateDeviceRegistration;
using SiaAdmin.Application.Features.Commands.User.CreateUser;
using SiaAdmin.Application.Features.Commands.User.DeleteUser;
using SiaAdmin.Application.Features.Commands.User.InsertOrUpdateUser;
using SiaAdmin.Application.Features.Commands.User.UpdateUserContactChannel;
using SiaAdmin.Application.Features.Commands.User.UpdateUserProfile;
using SiaAdmin.Application.Features.Commands.User.UpdateUserProfilePicture;
using SiaAdmin.Application.Features.Commands.User.UpdateUserReferenceCode;
using SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokenList;
using SiaAdmin.Application.Features.Queries.SiaUser.GetByPhoneNumberUser;
using SiaAdmin.Application.Features.Queries.User.GetListLastSeenAdet;
using SiaAdmin.Application.Features.Queries.User.GetSurveyUserList;
using SiaAdmin.Application.Features.Queries.User.GetUserProfile;
using SiaAdmin.Application.Features.Queries.User.GetUserProfilePicture;
using SiaAdmin.Application.Features.Queries.User.GetUserRecievedGiftsList;
using SiaAdmin.Application.Features.Queries.User.GetUserSelectIncentiveList;
using SiaAdmin.Application.Features.Queries.User.GetUserSurveyInfo;
using SiaAdmin.Application.Features.Queries.User.GetUserSurveyPoint;
using SiaAdmin.Application.Features.Queries.User.GetUserTransactionLogsList;
using SiaAdmin.Application.Interfaces.Firebase.Models;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
   
    public class UserController : BaseApiController
    {
        [HttpPost("CreateUser")]
        [NonAction]
        public async Task<IActionResult> CreateUser([FromQuery]CreateUserRequest createUserRequest)
        {
            var response = Mediator.Send(createUserRequest);
            return Ok(response);
        }
      
        /// <summary>
        /// Kullanıcı ekler eğer kullanıcı varsa günceller.
        /// </summary>
        [HttpPost("InsertOrUpdateUser")] 
        [NonAction]
        public async Task<IActionResult> InsertOrUpdateUser([FromBody] InsertOrUpdateUserRequest insertOrUpdateUserRequest)
        {

            var response = await Mediator.Send(insertOrUpdateUserRequest);
            return Ok(response);
        }
        /// <summary>
        /// Kullanıcın guid bilgilerini response olarak döner.
        /// </summary>
        [HttpGet("GetUserInfo")]
        [NonAction]
      
        public async Task<IActionResult> GetUserInfo([FromQuery] GetByPhoneNumberUserRequest getByPhoneNumberUserRequest)
        {

            var response = await Mediator.Send(getByPhoneNumberUserRequest);
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcı profil bilgilerini response olarak döner .
        /// </summary>
        [HttpGet("GetUserProfile")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new GetUserProfileRequest{UserGuid = userGuid});
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcıya ait atanmış anket bilgisini response olarak döner.
        /// </summary>
        [HttpGet("GetUserSurveyList")]
        [Authorize]
        public async Task<IActionResult> GetUserSurveyList()
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();

            var response = await Mediator.Send(new GetSurveyUserListRequest(){UserGUID = userGuid});
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcının kazanmış olduğu anket puanlarının toplamanı response olarak döner.
        /// </summary>
        [HttpGet("GetUserSurveyPoint")]
        [Authorize]
        public async Task<IActionResult> GetUserSurveyPoint( )
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();

            var response = await Mediator.Send(new GetUserSurveyPointRequest(){UserGUID = userGuid});
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcın kazanmış olduğu hediye bilgisi response olarak döner.
        /// </summary>
        [HttpGet("GetUserRecievedGifts")]
        [Authorize]
        public async Task<IActionResult> GetUserRecievedGifts( )
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new GetUserRecievedGiftsListRequest(){UserGUID = userGuid});
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcın kazanabileceği hediye bilgisi response olarak döner.
        /// </summary>
        [HttpGet("GetUserSelectableIncentives")]
        [Authorize]
        public async Task<IActionResult> GetUserSelectIncentives( )
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var respose = await Mediator.Send(new GetUserSelectIncentiveListRequest()
            {
                UserGUID = userGuid
            });
            return Ok(respose);
        }
        /// <summary>
        /// Kullanıcın tamamladığı anket ve puan bilgisi döner.
        /// </summary>
        [HttpGet("GetUserSurveyInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserSurveyInfo()
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var respose = await Mediator.Send(new GetUserSurveyInfoRequest()
            {
                UserGUID = userGuid
            });
            return Ok(respose);
        }
        /// <summary>
        /// Kullanıcın yaptığı puan harcamaları bilgisini response olarak döner.
        /// </summary>
        [HttpGet("GetUserTransactionLog")]
        [Authorize]
        public async Task<IActionResult> GetUserTransactionLogs()
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new GetUserTransactionLogsListRequest(){UserGUID = userGuid });
            return Ok(response); 
        }
        /// <summary>
        /// Kullanıcın profilini siler.
        /// </summary>
        [HttpGet("DeleteProfile")]
        [Authorize]
        public async Task<IActionResult> DeleteUserProfile()
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new DeleteUserRequest() { InternalGUID = Guid.Parse(userGuid) });
            return Ok(response);
        }
        /// <summary>
        /// Kullanıcın profil bilgilerini günceller.
        /// </summary>
        [HttpPost("UpdateUserProfil")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfil([FromQuery]UpdateUserProfilVM model)
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new UpdateUserProfileRequest()
            {
                Name = model.Name, Surname = model.Surname, Birthdate = model.Birthdate, Email = model.Email,
                Location = model.Location, Sex = model.Sex, InternalGuid = Guid.Parse(userGuid)
            });
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcın bildirim için üretilen token bilgisini kayıt eder. 
        /// </summary>
        [HttpPost("CreateDeviceTokenRegistration")]
        [Authorize]
        public async Task<IActionResult> CreateDeviceTokenRegistration(CreateDeviceToken token)
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new CreateDeviceRegistrationRequest()
            { 
                DeviceIdToken = token.Token,
                InternalGUID = Guid.Parse(userGuid)

            });
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcın bildirim için üretilen token bilgisini geri döndürür. 
        /// </summary>
        [HttpGet("GetDeviceTokenRegistrationList")]
        [Authorize]
        public async Task<IActionResult> GetDeviceTokenRegistrationList()
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new GetUserDeviceTokenListRequest(){InternalGUID = Guid.Parse(userGuid)});
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcının iletisim tercihlerini günceller. 
        /// </summary>
        [HttpPost("UpdateUserContactChannel")]
        [Authorize]
        public async Task<IActionResult> UpdateUserContactChannel(UpdateContactChannel request)
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new UpdateUserContactChannelRequest() { InternalGUID = Guid.Parse(userGuid),IsCheckedEmail = request.IsCheckedEmail,IsCheckedPhone = request.IsCheckedPhone,IsCheckedSms = request.IsCheckedSms});
            return Ok(response);
        }

        /// <summary>
        /// Kullanıcının profil resmini günceller. 
        /// </summary>
        [HttpPost("UpdateUserProfilePicture")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfilePicture(IFormFile file)
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString(); 
            var response = await Mediator.Send(new UpdateUserProfilePictureRequest(){InternalGUID = Guid.Parse(userGuid),Picture = file});
            return Ok(response);
        }
        /// <summary>
        /// Kullanıcının profil resmin yolunu döndürür. 
        /// </summary>
        [HttpGet("GetUserProfilePicture")]
        [Authorize] 
        public async Task<IActionResult> GetUserProfilePicture()
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString(); 
            var response = await Mediator.Send(new GetUserProfilePictureRequest() { InternalGUID = Guid.Parse(userGuid)});
            return Ok(response);
        }

        [HttpPost("UpdateReferenceCode")]
        [Authorize]
        public async Task<IActionResult> UpdateProfieRefrenceCode(UpdateReferenceCodeVM model)
        {
            string userGuid = HttpContext.Items["userGuid"]?.ToString();
            var response = await Mediator.Send(new UpdateUserReferenceCodeRequest() { ReferenceCode = model.ReferenceCode,InternalGUID = Guid.Parse(userGuid) });
            return Ok(response);
        }

        public class UpdateReferenceCodeVM
        {
            public string ReferenceCode { get; set; }
        }

        public class UpdateUserProfilVM
        {
            public string Email { get; set; }

            public string Name { get; set; }

            public string Surname { get; set; }

            public int Birthdate { get; set; }

            public int Sex { get; set; }

            public int Location { get; set; } 
        }

        public class CreateDeviceToken
        {
            public string Token { get; set; } 
        }

        public class UpdateContactChannel
        { 
            public bool IsCheckedSms { get; set; }
            public bool IsCheckedPhone { get; set; }
            public bool IsCheckedEmail { get; set; }
        }
    }
}
