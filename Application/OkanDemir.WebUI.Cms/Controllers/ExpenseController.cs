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
using OkanDemir.WebUI.Cms.Models;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        ExpenseBusiness _expenseBusiness;

        public ExpenseController(ExpenseBusiness _expenseBusiness)
        {
            this._expenseBusiness = _expenseBusiness;
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
            var response = _expenseBusiness.GetAll(new ExpenseFilterModel(dataTableParameters));

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
            return View(new ExpenseDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExpenseDto model, IFormFile file)
        {
            if(file != null)
            {
                var fileResponse = ImageHelper.UploadFile("expense", file);
                model.FilePath = fileResponse.Path;
            }
            
            var response = _expenseBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _expenseBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "Income", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ExpenseDto model, IFormFile file)
        {
            if (file != null)
            {
                var fileResponse = ImageHelper.UploadFile("expense", file);
                model.FilePath = fileResponse.Path;
            }

            var response = _expenseBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _expenseBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Active(int id)
        {
            var response = _expenseBusiness.Payment(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Passive(int id)
        {
            var response = _expenseBusiness.UnPayment(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
