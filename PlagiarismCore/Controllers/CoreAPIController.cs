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
    public class CoreAPIController : Controller
    {
        public UserManager<CommonAppUser> UserManager => HttpContext.GetOwinContext().GetUserManager<UserManager<CommonAppUser>>();
        public SignInManager<CommonAppUser, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<CommonAppUser, string>>();
        public RoleManager<IdentityRole, string> RoleManager => HttpContext.GetOwinContext().Get<RoleManager<IdentityRole, string>>();
        public MainContext Context => HttpContext.GetOwinContext().Get<MainContext>();


        #region API
        public JsonResult GetClasses()
        {
            var classes = Context.Classes.Select(x => new
            {
                ID = x.Id,
                Text = x.ClassName
            }).OrderBy(x => x.Text).ToList();
            return Json(classes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudents()
        {
            var role = RoleManager.FindByName("Student");
            var students = UserManager
                .Users
                .Where(x => x.Roles.Any(y => y.RoleId == role.Id))
                .Select(x => new
                {
                    ID = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    StudentID = x.UserName,
                    AssignedClass = x.Class != null ? x.Class.ClassName : "Unassigned",
                    Assignments = x.Assignments.Count
                }).ToList();
            return Json(new { data = students }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssignments()
        {
            List<Assignment> assignments = Context.Assignments.ToList();
            return Json(new { data = assignments }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudentAssignments(string id)
        {
            var role = RoleManager.FindByName("Student");
            CommonAppUser user = UserManager
                .Users
                .FirstOrDefault(x => x.Id.ToString().Equals(id, StringComparison.InvariantCultureIgnoreCase)
                    && x.Roles.Any(y => y.RoleId == role.Id));
            List<Assignment> assignments = user.Assignments.ToList();
            if (user == null)
            {
                return Json(new { Errors = new string[] { "User not found" } });
            }
            return Json(new { data = assignments }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudentSubmittedAssignments(string id)
        {
            var role = RoleManager.FindByName("Student");
            CommonAppUser user = UserManager
                .Users
                .FirstOrDefault(x => x.Id.ToString().Equals(id, StringComparison.InvariantCultureIgnoreCase)
                    && x.Roles.Any(y => y.RoleId == role.Id));
            List<SubmittedAssignment> assignments = user.SubmittedAssignments.ToList();
            if (user == null)
            {
                return Json(new { Errors = new string[] { "User not found" } });
            }
            return Json(new { data = assignments }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}