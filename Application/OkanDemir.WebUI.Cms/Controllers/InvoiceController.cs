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
    public class InvoiceController : Controller
    {
        InvoiceBusiness _invoiceBusiness;

        public InvoiceController(InvoiceBusiness _invoiceBusiness)
        {
            this._invoiceBusiness = _invoiceBusiness;
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
            var response = _invoiceBusiness.GetAll(new InvoiceFilterModel(dataTableParameters));

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
        public IActionResult Create(InvoiceDto model, IFormFile file)
        {
            if(file != null)
            {
                var fileResponse = ImageHelper.UploadFile("invoice", file);
                model.InvoiceFile = fileResponse.Path;
            }
            
            var response = _invoiceBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _invoiceBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "Income", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(InvoiceDto model, IFormFile file)
        {
            if (file != null)
            {
                var fileResponse = ImageHelper.UploadFile("invoice", file);
                model.InvoiceFile = fileResponse.Path;
            }

            var response = _invoiceBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _invoiceBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Active(int id)
        {
            var response = _invoiceBusiness.Payment(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Passive(int id)
        {
            var response = _invoiceBusiness.UnPayment(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
