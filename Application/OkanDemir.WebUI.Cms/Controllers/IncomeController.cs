using OkanDemir.WebUI.Cms.Authorize;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OkanDemir.Business;
using OkanDemir.WebUI.Cms.Helpers;
using OkanDemir.Business.Filters;
using OkanDemir.Dto;
using System.Diagnostics;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class IncomeController : Controller
    {
        IncomeBusiness _incomeBusiness;

        public IncomeController(IncomeBusiness _incomeBusiness)
        {
            this._incomeBusiness = _incomeBusiness;
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
            var response = _incomeBusiness.GetAll(new IncomeFilterModel(dataTableParameters));

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
            return View(new IncomeDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IncomeDto model, IFormFile file)
        {
            if (file != null)
            {
                var fileResponse = ImageHelper.UploadFile("income", file);
                model.FilePath = fileResponse.Path;
            }

            var response = _incomeBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _incomeBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "Income", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IncomeDto model, IFormFile file)
        {
            if (file != null)
            {
                var fileResponse = ImageHelper.UploadFile("income", file);
                model.FilePath = fileResponse.Path;
            }

            var response = _incomeBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _incomeBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Active(int id)
        {
            var response = _incomeBusiness.Payment(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Passive(int id)
        {
            var response = _incomeBusiness.UnPayment(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
