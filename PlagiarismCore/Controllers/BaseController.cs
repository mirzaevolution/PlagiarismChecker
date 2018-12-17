using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Plagiarism.DataLayer.Models;
using System.Web.Mvc;
using System.Threading.Tasks;
using PlagiarismCore.Models.IdentityModels;
using System.Web.Mvc.Filters;

namespace PlagiarismCore.Controllers
{
    public class BaseController : Controller
    {

        public UserManager<CommonAppUser> UserManager => HttpContext.GetOwinContext().GetUserManager<UserManager<CommonAppUser>>();
        public SignInManager<CommonAppUser, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<CommonAppUser, string>>();

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {

            if ((User.Identity != null && User.Identity.IsAuthenticated))
            {
                var userId = User.Identity.GetUserId();
                var exists = UserManager.FindById(userId);
                if (exists == null)
                {
                    SignInManager.AuthenticationManager.SignOut();
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Home/Index.cshtml"
                    };
                }
            }
        }

    }
}