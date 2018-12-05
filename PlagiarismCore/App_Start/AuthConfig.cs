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

namespace PlagiarismCore
{
    public class AuthConfig
    {
        public static void Configure(IAppBuilder app)
        {
            app.CreatePerOwinContext<MainContext>((options, owin) => new MainContext());
            app.CreatePerOwinContext<UserStore<CommonAppUser>>((options, owin) => new UserStore<CommonAppUser>(owin.Get<MainContext>()));
            app.CreatePerOwinContext<UserManager<CommonAppUser>>((options, owin) =>
            {
                var userManager = new UserManager<CommonAppUser>(owin.Get<UserStore<CommonAppUser>>());
                userManager.UserValidator = new UserValidator<CommonAppUser>(userManager);
                return userManager;
            });
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromDays(1),
                LoginPath = new PathString("/Authentication/Login"),
                LogoutPath = new PathString("/Authentication/Logout"),
                ReturnUrlParameter = "returnUrl"
            });
            
        }
    }
}