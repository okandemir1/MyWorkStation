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
    public class IncomeTypeController : Controller
    {
        IncomeTypeBusiness _incomeTypeBusiness;
        CacheHelper _cache;

        public IncomeTypeController(IncomeTypeBusiness _incomeTypeBusiness, CacheHelper _cache)
        {
            this._incomeTypeBusiness = _incomeTypeBusiness;
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
            var response = _incomeTypeBusiness.GetAll(new IncomeTypeFilterModel(dataTableParameters));

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
        public IActionResult Create(IncomeTypeDto model)
        {
            var response = _incomeTypeBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _incomeTypeBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "IncomeType", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IncomeTypeDto model)
        {
            var response = _incomeTypeBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _incomeTypeBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Active(int id)
        {
            var response = _incomeTypeBusiness.Active(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Passive(int id)
        {
            var response = _incomeTypeBusiness.Passive(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
