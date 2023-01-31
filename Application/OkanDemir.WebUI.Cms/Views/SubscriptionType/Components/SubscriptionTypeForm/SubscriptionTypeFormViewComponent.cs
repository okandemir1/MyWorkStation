using Microsoft.AspNetCore.Mvc;
using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Views.SubscriptionType.Components.SubscriptionTypeForm
{
    public class SubscriptionTypeFormViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke(string action,
            SubscriptionTypeDto model)
        {
            ViewBag.Action = action;
            return View(model);
        }

    }
}
