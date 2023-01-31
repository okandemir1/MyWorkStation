using Microsoft.AspNetCore.Mvc;
using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Views.IncomeType.Components.IncomeTypeForm
{
    public class IncomeTypeFormViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke(string action,
            IncomeTypeDto model)
        {
            ViewBag.Action = action;
            return View(model);
        }

    }
}
