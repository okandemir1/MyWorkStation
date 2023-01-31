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
    public class ArchiveCategoryController : Controller
    {
        ArchiveCategoryBusiness _archiveCategoryBusiness;
        CacheHelper _cache;

        public ArchiveCategoryController(ArchiveCategoryBusiness _archiveCategoryBusiness, CacheHelper _cache)
        {
            this._archiveCategoryBusiness = _archiveCategoryBusiness;
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
            var response = _archiveCategoryBusiness.GetAll(new ArchiveCategoryFilterModel(dataTableParameters));

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
        public IActionResult Create(ArchiveCategoryDto model)
        {
            var response = _archiveCategoryBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _archiveCategoryBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "ArchiveCategory", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArchiveCategoryDto model)
        {
            var response = _archiveCategoryBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _archiveCategoryBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Active(int id)
        {
            var response = _archiveCategoryBusiness.Active(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Passive(int id)
        {
            var response = _archiveCategoryBusiness.Passive(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
