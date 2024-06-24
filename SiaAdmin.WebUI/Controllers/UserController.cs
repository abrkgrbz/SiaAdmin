using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Features.Queries.User;
using System.Net;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using SiaAdmin.Application.Features.Commands.User.CreateUser;
using SiaAdmin.Application.DTOs.Account;

namespace SiaAdmin.WebUI.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseController
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        [HttpPost("/User/SiaLogin")]
        public async Task<IActionResult> UserLogin(GetUserRequest getUserRequest)
        {
            try
            {
                var user = await Mediator.Send(getUserRequest);
                List<Claim> userClaims = new List<Claim>();
                userClaims.Add(new Claim("RoleType", user.RoleType));
                userClaims.Add(new Claim(ClaimTypes.Name, user.User.Name + " " + user.User.Surname));
                userClaims.Add(new Claim(ClaimTypes.Role, user.RoleType));
                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.User.UserGUID.ToString()));

                var claimsIdentity =
                    new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return Json(new { response = HttpStatusCode.OK, message = "Başarıyla Giriş Yaptınız",returnRedirect=Url.Action("Index","Home") });
            }
            catch (Exception e)
            {
                return Json(new { response = HttpStatusCode.BadRequest, message = e.Message });
            }

        }

        [HttpPost("/User/SiaRegister")]
        public async Task<IActionResult> UserRegister(CreateUserRequest createUserRequest)
        {

            var createUser = await Mediator.Send(createUserRequest);
            if (createUser.Succeeded)
            {
                return Json(new { result = 200, message = createUser.Message });
            }
            return Json(new { message = createUser.Message });


        }
    }
}
