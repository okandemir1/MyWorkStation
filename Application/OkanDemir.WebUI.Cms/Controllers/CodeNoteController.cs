using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Business.Filters;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Authorize;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class CodeNoteController : Controller
    {
        CodeNoteBusiness _codeNoteBusiness;
        CacheHelper _cache;

        public CodeNoteController(CodeNoteBusiness _codeNoteBusiness, CacheHelper _cache)
        {
            this._codeNoteBusiness = _codeNoteBusiness;
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
            var response = _codeNoteBusiness.GetAll(new CodeNoteFilterModel(dataTableParameters));

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
        public IActionResult Create(CodeNoteDto model)
        {
            var response = _codeNoteBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _codeNoteBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "CodeCategory", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CodeNoteDto model)
        {
            var response = _codeNoteBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _codeNoteBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Detail(int id)
        {
            var data = _codeNoteBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "CodeCategory", new { q = "not_found_data" });

            return View(data);
        }
    }
}
