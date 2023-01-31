using Microsoft.AspNetCore.Mvc;
using OkanDemir.Business;
using OkanDemir.Dto;
using OkanDemir.WebUI.Cms.Helpers;

namespace OkanDemir.WebUI.Cms.Views.Subscription.Components.SubscriptionForm
{
    public class SubscriptionFormViewComponent : ViewComponent
    {
        SubscriptionTypeBusiness subscriptionTypeBusiness;
        public SubscriptionFormViewComponent(SubscriptionTypeBusiness subscriptionTypeBusiness)
        {
            this.subscriptionTypeBusiness = subscriptionTypeBusiness;
        }

        public IViewComponentResult Invoke(string action,
            SubscriptionDto model)
        {
            ViewBag.Action = action;
            ViewBag.Categories = subscriptionTypeBusiness.GetAllActiveList(User.GetUserId());
            return View(model);
        }

    }
}
