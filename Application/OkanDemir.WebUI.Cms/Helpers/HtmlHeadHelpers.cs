using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace OkanDemir.WebUI.Cms.Helpers
{
    public class HtmlHeadHelpers
    {
        IActionContextAccessor actionContextAccessor { get; set; }

        public HtmlHeadHelpers(IActionContextAccessor actionContextAccessor)
        {
            this.actionContextAccessor = actionContextAccessor;
        }

        public string Canonical(string action, string controller, object values = null)
        {
            var urlHelper = new UrlHelper(actionContextAccessor.ActionContext);
            var url = "https://okandemir.com";
            var canonical = url.TrimEnd('/')
                + urlHelper.Action(new UrlActionContext() { 
                    Controller = controller,
                    Action = action,
                    Values = values
                });

            return canonical;
        }
    }
}
