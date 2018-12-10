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
using System.Data.Entity;
using PlagiarismCore.Migrations;

namespace PlagiarismCore
{
    public class AuthConfig
    {
        public static void Configure(IAppBuilder app)
        {
            
            try
            {
                Database.SetInitializer<MainContext>(new NullDatabaseInitializer<MainContext>());
                app.CreatePerOwinContext<MainContext>((options, owin) => new MainContext());
                app.CreatePerOwinContext<UserStore<CommonAppUser>>((options, owin) => new UserStore<CommonAppUser>(owin.Get<MainContext>()));
                app.CreatePerOwinContext<UserManager<CommonAppUser>>((options, owin) =>
                {
                    var userManager = new UserManager<CommonAppUser>(owin.Get<UserStore<CommonAppUser>>());
                    userManager.UserValidator = new UserValidator<CommonAppUser>(userManager)
                    {
                        RequireUniqueEmail = true
                    };
                    //userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(4);
                    //userManager.MaxFailedAccessAttemptsBeforeLockout = 3;
                    //userManager.UserLockoutEnabledByDefault = true;

                    return userManager;
                });
                app.CreatePerOwinContext<SignInManager<CommonAppUser, string>>((options, owin) =>
                     new SignInManager<CommonAppUser, string>(
                                 owin.GetUserManager<UserManager<CommonAppUser>>(),
                                 owin.Authentication
                         )
                );
                app.CreatePerOwinContext<RoleStore<IdentityRole>>((options, owin) => new RoleStore<IdentityRole>(new MainContext()));
                app.CreatePerOwinContext<RoleManager<IdentityRole, string>>((options, owin) => new RoleManager<IdentityRole, string>(owin.Get<RoleStore<IdentityRole>>()));
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,

                    LoginPath = new PathString("/Authentication/Login"),
                    LogoutPath = new PathString("/Authentication/Logout"),
                    ReturnUrlParameter = "returnUrl"
                });
            }
            catch (Exception ex)
            {
                throw new CustomException
                {
                    Errors = new List<string>
                    {
                        ex.Message,
                        ex?.InnerException?.Message
                    }
                };
            }
        }
    }
}