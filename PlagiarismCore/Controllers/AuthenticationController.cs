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

namespace PlagiarismCore.Controllers
{
    public class AuthenticationController : BaseController
    {
        

        public ActionResult Login(string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl = "/")
        {
            List<string> errors = new List<string>();
            bool sysError = false;
            try
            {
                bool remember = (!string.IsNullOrEmpty(model.RememberMe) && model.RememberMe.ToLower().Equals("on")) ? true : false;
                if (ModelState.IsValid)
                {
                    SignInStatus loginStatus = await SignInManager
                        .PasswordSignInAsync(model.UserName, model.Password, remember, false);
                    switch (loginStatus)
                    {
                        case SignInStatus.Failure:
                            {
                                ModelState.AddModelError("", "Invalid credentials");
                                return View(model);
                            }
                    }
                }
                else
                {
                    errors.Add("Please complete all field requirements!");
                }
            }
            catch (Exception ex)
            {
                sysError = true;
                errors.Add(ex.Message);
                errors.Add(ex?.InnerException?.Message);

            }
            if (sysError)
            {
                throw new CustomException
                {
                    Errors = errors
                };
            }
            else if (!sysError && errors.Count > 0)
            {
                string error = errors.Aggregate((curr, next) => $"{curr}\n{next}");
                ModelState.AddModelError("", error);
                return View(model);
            }
            else
            {
                return Redirect(returnUrl);
            }
        }


        [Authorize]
        public ActionResult Logout()
        {
            SignInManager.AuthenticationManager.SignOut(SignInManager.AuthenticationType);
            return RedirectToAction("Index", "Home");
        }
    }
}