using OkanDemir.WebUI.Cms.Authorize;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OkanDemir.Business;
using OkanDemir.WebUI.Cms.Helpers;
using OkanDemir.Business.Filters;
using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class ArchiveController : Controller
    {
        ArchiveBusiness _archiveBusiness;
        CacheHelper _cache;

        public ArchiveController(ArchiveBusiness _archiveBusiness, CacheHelper _cache)
        {
            this._archiveBusiness = _archiveBusiness;
            this._cache = _cache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableParameters dataTableParameters)
        {
            dataTableParameters.UserId = User.GetUserId();
            var response = _archiveBusiness.GetAll(new ArchiveFilterModel(dataTableParameters));

            return Json(
            new
            {
                draw = dataTableParameters.Draw,
                recordsFiltered = response.RecordsFiltered,
                recordsTotal = response.TotalCount,
                data = response.Data
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArchiveDto model)
        {
            var response = _archiveBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _archiveBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "Archive", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArchiveDto model)
        {
            var response = _archiveBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _archiveBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Active(int id)
        {
            var response = _archiveBusiness.Active(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Passive(int id)
        {
            var response = _archiveBusiness.Passive(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpPost]
        public JsonResult ShowPass(int id, string key)
        {
            var response = _archiveBusiness.ShowPassword(User.GetUserId(), id,key);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors, instance = response.Instance });
        }
    }
}
