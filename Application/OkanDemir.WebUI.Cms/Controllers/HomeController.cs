using OkanDemir.WebUI.Cms.Authorize;
using OkanDemir.WebUI.Cms.Helpers;
using OkanDemir.WebUI.Cms.Models;
using OkanDemir.Data.Repository;
using OkanDemir.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRepository<User> _userRepository;
        CacheHelper _cacheHelper;
        public HomeController(CacheHelper _cacheHelper, IRepository<User> _userRepository)
        {
            this._cacheHelper = _cacheHelper;
            this._userRepository = _userRepository;
        }
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
