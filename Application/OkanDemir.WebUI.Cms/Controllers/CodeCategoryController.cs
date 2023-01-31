using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Business.Filters;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Authorize;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class CodeCategoryController : Controller
    {
        CodeCategoryBusiness _codeCategoryBusiness;
        CacheHelper _cache;

        public CodeCategoryController(CodeCategoryBusiness _codeCategoryBusiness, CacheHelper _cache)
        {
            this._codeCategoryBusiness = _codeCategoryBusiness;
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
            var response = _codeCategoryBusiness.GetAll(new CodeCategoryFilterModel(dataTableParameters));

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
        public IActionResult Create(CodeCategoryDto model)
        {
            var response = _codeCategoryBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _codeCategoryBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "CodeCategory", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CodeCategoryDto model)
        {
            var response = _codeCategoryBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _codeCategoryBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Active(int id)
        {
            var response = _codeCategoryBusiness.Active(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Passive(int id)
        {
            var response = _codeCategoryBusiness.Passive(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
