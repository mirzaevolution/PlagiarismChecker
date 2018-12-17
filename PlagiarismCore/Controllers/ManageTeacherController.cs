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
    public class ManageTeacherController : BaseController
    {
        public RoleManager<IdentityRole, string> RoleManager => HttpContext.GetOwinContext().Get<RoleManager<IdentityRole, string>>();
        public MainContext Context => HttpContext.GetOwinContext().Get<MainContext>();
        public ActionResult StudentAdministration()
        {
            return View();
        }
        public ActionResult StudentDetail(string id)
        {
            var role = RoleManager.FindByName("Student");
            CommonAppUser user = UserManager
                .Users
                .FirstOrDefault(x => x.Id.ToString().Equals(id, StringComparison.InvariantCultureIgnoreCase)
                    && x.Roles.Any(y => y.RoleId == role.Id));
            if (user == null)
            {
                throw new CustomException
                {
                    Errors = new List<string>() { "Student not found" }
                };
            }
            StudentModel model = new StudentModel
            {
                ID = user.Id.ToString(),
                Email = user.Email,
                FullName = user.FullName,
                ClassID = user.Class.Id,
                StudentID = user.UserName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentDetail(StudentModel model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if (ModelState.IsValid)
                {

                    CommonAppUser userAccount = await UserManager.FindByIdAsync(model.ID);
                    if (userAccount == null)
                    {
                        throw new CustomException
                        {
                            Errors = new List<string>
                            {
                                "Posted data is invalid"
                            }
                        };
                    }
                    userAccount.FullName = model.FullName;
                    userAccount.UserName = model.StudentID;
                    userAccount.Email = model.Email;
                    Class user = Context.Classes.FirstOrDefault(x => x.Id.ToString() == model.ClassID.ToString());
                    userAccount.Class = user;
                    IdentityResult updateResult = await UserManager.UpdateAsync(userAccount);
                    if (updateResult.Succeeded)
                    {
                        ViewBag.Message = "Data has been updated successfully";
                    }
                    else
                    {
                        success = true;
                        errors = errors.Concat(updateResult.Errors).ToList();
                    }
                }
                else
                {
                    errors.Add("Complete all fields!");
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
                errors.Add(ex.InnerException?.Message);
            }
            if (success && errors.Count == 0)
            {
                return View(model);
            }
            else if (success && errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    ModelState.AddModelError("", error);       

                }
                return View(model);
            }
            else
            {
                throw new CustomException
                {
                    Errors = errors
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteStudent(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new CustomException
                {
                    Errors = new List<string>
                    {
                        "User not found"
                    }
                };
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new CustomException
                {
                    Errors = new List<string>
                    {
                        "User not found"
                    }
                };
            }
            foreach (var submittedAssigment in user.SubmittedAssignments.ToList())
            {
                if (!Context.SubmittedAssignments.Local.Contains(submittedAssigment))
                {
                    Context.SubmittedAssignments.Attach(submittedAssigment);
                }
                Context.Entry(submittedAssigment).State = System.Data.Entity.EntityState.Deleted;
            }
            Context.SaveChanges();
            await UserManager.DeleteAsync(user);
            return RedirectToAction("StudentAdministration");
        }


        public ActionResult AssignmentAdministration()
        {
            return View();
        }
        public ActionResult AssignmentDetail(string id, string subjectName)
        {
            ViewBag.UserID = User.Identity.GetUserId();
            ViewBag.ID = id;
            ViewBag.SubjectName = subjectName;
            return View();
        }
    
        public async Task<ActionResult> MyProfile()
        {
            CommonAppUser commonAppUser =
                await UserManager.FindByIdAsync(
                            User.Identity.GetUserId<string>()
                        );

            return View(new TeacherProfile
            {
                FullName = commonAppUser.FullName,
                Email = commonAppUser.Email
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MyProfile(TeacherProfile model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if (ModelState.IsValid)
                {
                    CommonAppUser commonAppUser =
                                    await UserManager.FindByIdAsync(
                                                User.Identity.GetUserId<string>()
                                            );
                    commonAppUser.FullName = model.FullName;
                    commonAppUser.Email = model.Email;
                    var updateResult = await UserManager.UpdateAsync(commonAppUser);

                    if (updateResult.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.NewPassword) &&
                            !string.IsNullOrEmpty(model.CurrentPassword))
                        {
                            updateResult = await UserManager.ChangePasswordAsync(commonAppUser.Id,
                                    model.CurrentPassword, model.NewPassword
                                );
                            if (!updateResult.Succeeded)
                            {
                                success = false;
                                errors.Add("Error while changing user password");
                            }
                            else
                            {

                                ViewBag.Message = "Profile has been saved successfully";
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Profile has been saved successfully";
                        }
                    }
                    else
                    {
                        success = false;
                        errors.Add("Error while updating the profile");
                        errors.AddRange(updateResult.Errors);
                    }
                }
                else
                {
                    success = false;
                    errors.Add("Please complete all fields");
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors.Add(ex.ToString());
            }
            model.CurrentPassword = "";
            model.NewPassword = "";
            if (success && errors.Count == 0)
            {
                return View(model);
            }
            else
            {
                foreach (string error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }
        }
    }
}