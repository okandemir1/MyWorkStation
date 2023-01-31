using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Views.Income.Components.IncomeForm
{
    public class ExpenseFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string action)
        {
            ViewBag.Action = action;
            return View();
        }

    }
}
