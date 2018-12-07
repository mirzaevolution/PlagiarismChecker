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
        public JsonResult GetAssignments(string id)
        {
            var role = RoleManager.FindByName("Student");
            CommonAppUser user = UserManager
                .Users
                .FirstOrDefault(x => x.Id.ToString().Equals(id, StringComparison.InvariantCultureIgnoreCase)
                    && x.Roles.Any(y => y.RoleId == role.Id));
            var userAssignments = user
                .Assignments
                .Select(x => new
                {
                    x.Id,
                    x.AssignmentName
                }).ToList();
            var assignments = Context.Assignments.Select(x => new
            {
                x.Id,
                x.AssignmentName
            }).ToList().Except(userAssignments).ToList();
            return Json(assignments, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudentAssignments(string id)
        {
            var role = RoleManager.FindByName("Student");
            CommonAppUser user = UserManager
                .Users
                .FirstOrDefault(x => x.Id.ToString().Equals(id, StringComparison.InvariantCultureIgnoreCase)
                    && x.Roles.Any(y => y.RoleId == role.Id));
            var assignments = user
                .Assignments
                .Select(x => new
                {
                    x.Id,
                    x.AssignmentName
                }).ToList();
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
            var assignments = user
                .SubmittedAssignments
                .Select(x => new
                {
                    AssignmentName = x.Assignment!=null?x.Assignment.AssignmentName:"Unknown",
                    x.Description,
                    x.PercentageInteger,
                    Status = x.IsAccepted ? "Accepted":"Rejected",
                    x.SubmissionDate
                })
                .ToList();


            if (user == null)
            {
                return Json(new { Errors = new string[] { "User not found" } });
            }
            return Json(new { data = assignments }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllAssignments()
        {
            var assignments = Context.Assignments.Select(x => new
            {
                x.Id,
                x.AssignmentName
            }).ToList();
            return Json(new { data= assignments }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult PostStudentAssignment(string userId, string assignmentId)
        {
            bool success = true;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(assignmentId))
            {
                success = false;
                errors.Add("Missing required parameter(s)");
            }
            try
            {
                if (success)
                {
                    var student = UserManager.FindById(userId);
                    if (student != null)
                    {
                        var assignment = Context.Assignments.Find(Guid.Parse(assignmentId));
                        if (assignment != null)
                        {
                            student.Assignments.Add(assignment);
                            var result =  UserManager.Update(student);
                            if(!result.Succeeded)
                            {
                                success = false;
                                errors.AddRange(result.Errors);
                            }
                        }
                        else
                        {
                            success = false;
                            errors.Add("Assignment not found");
                        }
                    }
                    else
                    {
                        success = false;
                        errors.Add("User not found");
                    }
                }
            }
            catch(Exception ex)
            {
                success = false;
                errors.Add(ex.ToString());
            }
            return Json(new { Success = success, Errors = errors });
        }

        [HttpPost]
        public JsonResult DeleteAssignment(string userId, string assignmentId)
        {
            bool success = true;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(assignmentId))
            {
                success = false;
                errors.Add("Missing required parameter(s)");
            }
            try
            {
                if (success)
                {
                    var student = UserManager.FindById(userId);
                    if (student != null)
                    {
                        var assignment = Context.Assignments.Find(Guid.Parse(assignmentId));
                        if (assignment != null)
                        {
                            student.Assignments.Remove(assignment);
                            var result = UserManager.Update(student);
                            if (!result.Succeeded)
                            {
                                success = false;
                                errors.AddRange(result.Errors);
                            }
                        }
                        else
                        {
                            success = false;
                            errors.Add("Assignment not found");
                        }
                    }
                    else
                    {
                        success = false;
                        errors.Add("User not found");
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors.Add(ex.ToString());
            }
            return Json(new { Success = success, Errors = errors });
        }
        #endregion
    }
}