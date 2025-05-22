using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        private readonly IConfiguration Configuration;

        public SiaUserController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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
            getByGuidSiaUserRequest.SurveyUserGuid = Guid.Parse(guid);
            var response = await Mediator.Send(getByGuidSiaUserRequest);
            model.Active = response.GetUserByGuidViewModel.Active;
            model.Email = response.GetUserByGuidViewModel.Email;
            model.LastIP = response.GetUserByGuidViewModel.LastIP;
            model.LastLogin = response.GetUserByGuidViewModel.LastLogin;
            model.LoginCount = response.GetUserByGuidViewModel.LoginCount;
            model.Msisdn = response.GetUserByGuidViewModel.Msisdn;
            model.Name = response.GetUserByGuidViewModel.Name;
            model.ProfilPuani = response.GetUserByGuidViewModel.ProfilPuani;
            model.RegistrationDate = response.GetUserByGuidViewModel.RegistrationDate;
            model.Surname = response.GetUserByGuidViewModel.Surname;
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


        [HttpGet]
        public IActionResult SwitchToUserProfile(string msisdn)
        {
            try
            {
                string varState = "300";
                int? resultCode = 0;
                string resultMessage = "Mesaj";
                string email = "";
                string regionCode = "+90";
                 
                TempData["FL_msisdn"] = msisdn;
                TempData["FL_email"] = email;
                TempData["FL_regionCode"] = regionCode;
                TempData["FL_resultMessage"] = resultMessage;
                TempData["FL_resultCode"] = resultCode.ToString();

                string connectionString = Configuration.GetConnectionString("SiaAdminSql").Replace("&amp;", "&");
                string userGuid = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string queryString = @"
                select top 1 * from Users where [RegionCode] = @r and [Msisdn] = @m and Active = 1
            ";
                    SqlCommand command = new SqlCommand(queryString, connection);

                    command.Parameters.AddWithValue("@r", regionCode);
                    command.Parameters.AddWithValue("@m", msisdn);
                    command.Parameters.AddWithValue("@e", email);

                    connection.Open();

                    try
                    {
                        SqlDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            if (dataReader["InternalGUID"] != null && !Convert.IsDBNull(dataReader["InternalGUID"]))
                            {
                                userGuid = dataReader["InternalGUID"].ToString();
                            }
                        }
                        dataReader.Close();
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Kullanıcı verileri alınırken bir hata oluştu: " + ex.Message;
                        return RedirectToAction("UserList");
                    }
                }

                if (string.IsNullOrEmpty(userGuid))
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı veya aktif değil.";
                    return RedirectToAction("UserList");
                }

                TempData["FL_guid"] = userGuid;

                string zamanAnahtar = DateTime.UtcNow.ToString();

                string secretKey = "EnGizLIKeyBizdeBulunurKiN3";
                string hashValue = HesaplaKey(secretKey, userGuid);

                string cookieValue = $"guid={userGuid}&region={regionCode}&msisdn={msisdn}&lastvisit={zamanAnahtar}&q={hashValue}";

                Response.Cookies.Append("SiaLive", cookieValue, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(30)
                });
                 
                string redirectPage;
                switch (varState)
                {
                    case "100":
                        redirectPage = "Iletisim.aspx";
                        break;
                    case "200":
                        redirectPage = "Gizlilik.aspx";
                        break;
                    case "300":
                        redirectPage = "Default.aspx";
                        break;
                    case "400":
                        redirectPage = "KullanimKosul.aspx";
                        break;
                    case "500":
                        redirectPage = "Profil.aspx";
                        break;
                    case "600":
                        redirectPage = "Hediye.aspx";
                        break;
                    case "700":
                        redirectPage = "SSS.aspx";
                        break;
                    case "800":
                        redirectPage = "UyelikKalite.aspx";
                        break;
                    default:
                        redirectPage = "Default.aspx";
                        break;
                }
                 
                return Redirect($"https://sialive.siapanel.com/{redirectPage}");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İşlem sırasında bir hata oluştu: " + ex.Message;
                return RedirectToAction("UserList");
            }
        }
         
        private string HesaplaKey(string key, string message)
        {
            string value = "";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);

            using (HMACSHA256 hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] messageBytes = encoding.GetBytes(message);
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

                value = ByteToString(hashmessage);
            }
            return value.ToLower();
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex formatı
            }
            return sbinary;
        }

    }
}
