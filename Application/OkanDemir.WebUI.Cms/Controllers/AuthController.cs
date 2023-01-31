using OkanDemir.WebUI.Cms.Authorize;
using OkanDemir.WebUI.Cms.Helpers;
using OkanDemir.WebUI.Cms.Models;
using OkanDemir.Data.Repository;
using OkanDemir.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OkanDemir.Business;
using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Controllers
{
    public class AuthController : Controller
    {
        UserBusiness _userBusiness;
        public AuthController(UserBusiness _userBusiness)
        {
            this._userBusiness = _userBusiness;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginRequestDto model)
        {
            var response = _userBusiness.Login(model);
            if(!response.IsSucceed)
                return Json(new { isSucceed = false, message = response.Message, title = "Dikkat", errors = response.Errors });

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, response.Instance.Fullname),
                    new Claim(ClaimTypes.NameIdentifier, response.Instance.Id.ToString()),
                    new Claim(ClaimTypes.Role, response.Instance.RoleId.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddHours(30),
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Json(new { isSucceed = true, message = "Giriş Yapıldı.", title = "İşlem Başarılı", redirect = "/Home/Index" });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(RegisterRequestDto model)
        {
            var response = _userBusiness.Register(model);
            if (!response.IsSucceed)
                return Json(new { isSucceed = false, message = response.Message, title = "Dikkat", errors = response.Errors });

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, response.Instance.Fullname),
                    new Claim(ClaimTypes.NameIdentifier, response.Instance.Id.ToString()),
                    new Claim(ClaimTypes.Role, response.Instance.RoleId.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddHours(30),
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Json(new { isSucceed = true, message = "Giriş Yapıldı.", title = "İşlem Başarılı", redirect = "/Home/Index" });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
        }
    }
}
