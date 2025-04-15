using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Authorize;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class TodoProjectController : Controller
    {
        TodoBusiness _todoBusiness;
        CacheHelper _cache;

        public TodoProjectController(TodoBusiness _todoBusiness, CacheHelper _cache)
        {
            this._todoBusiness = _todoBusiness;
            this._cache = _cache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = _todoBusiness.GetAllTodoProjectsList(User.GetUserId());
            return View(list);
        }
        

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TodoProjectDto model)
        {
            var response = _todoBusiness.ProjectCreate(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _todoBusiness.ProjectGet(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "ArchiveCategory", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TodoProjectDto model)
        {
            var response = _todoBusiness.ProjectUpdate(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _todoBusiness.ProjectDelete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
