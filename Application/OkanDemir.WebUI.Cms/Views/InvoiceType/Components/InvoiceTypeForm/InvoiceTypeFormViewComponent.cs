using Microsoft.AspNetCore.Mvc;
using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Views.IncomeType.Components.IncomeTypeForm
{
    public class InvoiceTypeFormViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke(string action,
            InvoiceTypeDto model)
        {
            ViewBag.Action = action;
            return View(model);
        }

    }
}
