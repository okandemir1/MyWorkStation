using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Business.Filters;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Authorize;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Controllers
{
    [Authorize]
    public class SubscriptionTypeController : Controller
    {
        SubscriptionTypeBusiness _subscriptionTypeBusiness;

        public SubscriptionTypeController(SubscriptionTypeBusiness _subscriptionTypeBusiness)
        {
            this._subscriptionTypeBusiness = _subscriptionTypeBusiness;
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
            var response = _subscriptionTypeBusiness.GetAll(new SubscriptionTypeFilterModel(dataTableParameters));

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
        public IActionResult Create(SubscriptionTypeDto model)
        {
            var response = _subscriptionTypeBusiness.Create(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        public IActionResult Edit(int id)
        {
            var data = _subscriptionTypeBusiness.Get(User.GetUserId(), id);
            if (data == null)
                return RedirectToAction("Index", "IncomeType", new { q = "not_found_data" });

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubscriptionTypeDto model)
        {
            var response = _subscriptionTypeBusiness.Update(model);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var response = _subscriptionTypeBusiness.Delete(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Active(int id)
        {
            var response = _subscriptionTypeBusiness.Active(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }

        [HttpGet]
        public JsonResult Passive(int id)
        {
            var response = _subscriptionTypeBusiness.Passive(User.GetUserId(), id);
            return Json(new { isSucceed = response.IsSucceed, message = response.Message, errors = response.Errors });
        }
    }
}
