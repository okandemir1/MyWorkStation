using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Views.Income.Components.IncomeForm
{
    public class InvoiceFormViewComponent : ViewComponent
    {
        InvoiceTypeBusiness invoiceTypeBusiness;
        public InvoiceFormViewComponent(InvoiceTypeBusiness invoiceTypeBusiness)
        {
            this.invoiceTypeBusiness = invoiceTypeBusiness;
        }

        public IViewComponentResult Invoke(string action,
            InvoiceDto model)
        {
            ViewBag.Action = action;
            ViewBag.Categories = invoiceTypeBusiness.GetAllActiveList(User.GetUserId());
            return View(model);
        }

    }
}
