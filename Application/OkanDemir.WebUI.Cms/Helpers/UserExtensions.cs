using System;
using System.Security.Claims;
using System.Security.Principal;

namespace OkanDemir.WebUI.Cms.Helpers
{
    public static class UserExtensions
    {
        public static int GetUserId(this IPrincipal user)
        {
            var userClaim = user as ClaimsPrincipal;
            return Convert.ToInt32(userClaim?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        }
        public static string GetAppUserId(this IPrincipal user)
        {
            var userClaim = user as ClaimsPrincipal;
            return userClaim?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        }
    }
}