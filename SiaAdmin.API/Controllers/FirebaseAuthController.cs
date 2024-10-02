using System.Net;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.OTPHistory.CreateOTPHistory;
using SiaAdmin.Application.Features.Commands.User.InsertOrUpdateUser;
using SiaAdmin.Application.Features.Queries.OTPHistory.VerifyOTPHistory;
using SiaAdmin.Application.Features.Queries.SiaUser.GetByPhoneNumberUser;
using SiaAdmin.Application.Features.Queries.User.SendNotifactionUser;
using SiaAdmin.Application.Interfaces.Firebase.Models;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Infrastructure.Services;

namespace SiaAdmin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseAuthController : BaseApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly FirebaseService _firebaseService;
        public FirebaseAuthController(FirebaseService firebaseService, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _firebaseService = firebaseService;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpGet]
        [Authorize] 
        public async Task<IActionResult> Get()
        {
            return Ok("You are authenticated");
        }
        /// <summary>
        /// Kullanıcıyı ekler eğer kullanıcı varsa günceller ve kullanıcıya onay kodunu gönderir.
        /// </summary> 
        [HttpPost("sendVerificationCode")]
        [AllowAnonymous]
        public async Task<IActionResult> SendVerificationCode([FromBody] SendVertifactionCodeModel request)
        {
            var userResult = await Mediator.Send(new InsertOrUpdateUserRequest(){Phone = request.Phone,Browser = request.Browser,RegionCode = request.RegionCode,Ip = request.Ip});
            if (userResult.IsSucceded)
            {
                var response = await Mediator.Send(new CreateOTPHistoryRequest() { PhoneNumber = request.Phone, IpAdress = request.Ip, Browser = request.Browser });
                return Ok(response);
            }

            return Ok(userResult);


        }

        [HttpPost("verifyVericiationCode")]
        public async Task<IActionResult> VerifyVericiationCode([FromBody] VerifyCodeRequest request)
        {

            var isVerifyCode = await Mediator.Send(new VerifyOTPHistoryRequest
            { Code = request.Code, PhoneNumber = request.PhoneNumber });
            if (isVerifyCode.IsVerify)
            {
                var user = await Mediator.Send(new GetByPhoneNumberUserRequest { PhoneNumber = request.PhoneNumber });
                var users = new List<ImportUserRecordArgs>()
                {
                    new ImportUserRecordArgs()
                    {
                        Uid = user.GetUserPhoneNumberViewModel.UserGUID,
                        DisplayName = "SiaLive User",
                        PhoneNumber = user.GetUserPhoneNumberViewModel.PhoneNumber,
                        CustomClaims = new Dictionary<string, object>()
                        {
                            { "user", true }, // set this user as admin
                        },
                    }
                };
                if (!await ImportOrUpdateUser(users))
                {
                    return Ok(new { response = "Beklenmedik bir hata!" });
                }

                var customToken = await _firebaseService.CreateCustomTokenAsync(user.GetUserPhoneNumberViewModel.UserGUID);

                try
                {
                    var signInResponse = await _firebaseService.SignInWithCustomTokenAsync(customToken, true);

                    return Ok(new { data = signInResponse });
                }
                catch (HttpRequestException ex)
                {
                    return BadRequest(new { error = ex.Message });
                }

            }
            return Ok(new { response = "Telefon numaranız veya Onay kodunuz hatalı!" });
        }

        private async Task<bool> ImportOrUpdateUser(List<ImportUserRecordArgs> user)
        {
            foreach (var item in user)
            {
                try
                {
                    UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserByPhoneNumberAsync(item.PhoneNumber);
                    if (!userRecord.Uid.Equals(item.Uid))
                    {
                        await FirebaseAuth.DefaultInstance.DeleteUserAsync(userRecord.Uid);
                        UserRecord updateUser = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs()
                        {
                            PhoneNumber = item.PhoneNumber,
                            Uid = item.Uid,
                            DisplayName = item.DisplayName,
                            Email = item.Email,

                        });
                        return true;
                    }

                    if (userRecord.PhoneNumber!=null)
                    {
                        return true;
                    }
                    

                }
                catch (Exception e)
                {
                    var userImportResponse = await _firebaseService.ImportUsersAsync(user);
                    return true;
                }
                 
            }

            return false;
        }

        [HttpPost("GetRefreshToken")]
        public async Task<IActionResult> GetRefreshToken([FromBody] GetRefreshToken refreshToken)
        {
            var result = await _firebaseService.GetTokenWithRefreshToken(refreshToken.RefreshToken);
            return Ok(result);
        }

        [HttpPost("SendNotificationUsers")]
        [AllowAnonymous]
        public async Task<IActionResult> SendNotifications([FromBody]SendNotificationData request)
        {
            var result =
                await _firebaseService.SendMessagesToMultipleDevice(request.tokenList, request.title, request.body,
                    request.datas);
            return Ok(result);
        }
        public class SendNotificationData
        {
            public List<string> tokenList { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public Dictionary<string,string> datas { get; set; }
        }
        
        public class SendVertifactionCodeModel
        {
            public string RegionCode { get; set; }
            public string Phone { get; set; }
            public string Browser { get; set; }
            public string Ip { get; set; }
        }

        #region Kullanım Dışı

        [NonAction]
        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] InsertOrUpdateUserRequest model)
        {

            try
            {
                var users = new List<ImportUserRecordArgs>()
                {
                    new ImportUserRecordArgs()
                    {
                        Uid = "some-uid2",
                        DisplayName = "John Doe",
                        PhoneNumber = "+11234567771",
                        CustomClaims = new Dictionary<string, object>()
                        {
                            { "user", true }, // set this user as admin
                        },
                    },
                };

                UserImportResult result = await FirebaseAuth.DefaultInstance.ImportUsersAsync(users);
                foreach (ErrorInfo indexedError in result.Errors)
                {
                    Console.WriteLine($"Failed to import user: {indexedError.Reason}");
                }
                return Ok(new { token = "signInResponse.IdToken" });
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [NonAction]
        [HttpPost("signInWithCustomToken")]
        public async Task<IActionResult> SignInWithCustomToken([FromBody] LookupRequest token)
        {
            try
            {
                var signInResponse = await _firebaseService.SignInWithCustomTokenAsync(token.IdToken, true);

                return Ok(new { data = signInResponse });
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [NonAction]
        [HttpPost("verifyIdToken")]
        public async Task<IActionResult> verifyIdToken([FromBody] LookupRequest token)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(token.IdToken);
                string uid = decodedToken.Uid;

                return Ok(new { uid = uid });
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [NonAction]

        [HttpPost("createCustomToken")]
        public async Task<IActionResult> createCustomToken([FromBody] LoginModel model)
        {
            try
            {

                var uid = "evGVTPwsIde6h4MVO39KwIKJXle2";

                var additionalClaims = new Dictionary<string, object>()
                {
                    { "phoneNumber", "+9057512578" }
                };

                string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid, additionalClaims);

                return Ok(new { token = customToken });
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        private async Task<UserRecord> GetUserWithIdAsync(string uid)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                return userRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion
    }
}
