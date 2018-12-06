using System.Web;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Plagiarism.DataLayer.Models;

namespace System.Web
{
    public static class IdentityHtmlHelpers
    {
        public static string GetFullName(this IIdentity identity)
        {
            if (identity != null && identity.IsAuthenticated)
            {
                string userId = identity.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    UserManager<CommonAppUser> userManager =
                        HttpContext.Current.GetOwinContext().GetUserManager<UserManager<CommonAppUser>>();
                    CommonAppUser userAccount = userManager.FindById(userId);
                    if (userAccount != null)
                    {
                        return userAccount.FullName;
                    }
                }
            }
            return string.Empty;
        }
    }
}