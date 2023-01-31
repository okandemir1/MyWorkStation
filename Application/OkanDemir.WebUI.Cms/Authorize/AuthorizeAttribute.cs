using Microsoft.AspNetCore.Mvc;

namespace OkanDemir.WebUI.Cms.Authorize
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() 
            : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { };
        }
    }
}
