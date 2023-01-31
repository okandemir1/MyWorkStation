using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Views.Income.Components.IncomeForm
{
    public class IncomeFormViewComponent : ViewComponent
    {
        IncomeTypeBusiness incomeTypeBusiness;
        public IncomeFormViewComponent(IncomeTypeBusiness incomeTypeBusiness)
        {
            this.incomeTypeBusiness = incomeTypeBusiness;
        }

        public IViewComponentResult Invoke(string action,
            IncomeDto model)
        {
            ViewBag.Action = action;
            ViewBag.Categories = incomeTypeBusiness.GetAllActiveList(User.GetUserId());
            return View(model);
        }

    }
}
