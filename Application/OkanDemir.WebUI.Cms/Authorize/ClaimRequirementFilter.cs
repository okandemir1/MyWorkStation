using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OkanDemir.WebUI.Cms.Authorize
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Rol yapısı ilerde devreye girebilir şimdilik dursun
            var role = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Role);
            if (role == null)
            {
                context.Result = new RedirectResult("/Auth/Login");
                return;
            }
            
            return;
        }
    }
}
