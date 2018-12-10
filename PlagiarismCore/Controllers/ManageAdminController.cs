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

    [Authorize(Roles = "Admin")]
    public class ManageAdminController : Controller
    {
        public UserManager<CommonAppUser> UserManager => HttpContext.GetOwinContext().GetUserManager<UserManager<CommonAppUser>>();
        public SignInManager<CommonAppUser, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<CommonAppUser, string>>();
        public RoleManager<IdentityRole, string> RoleManager => HttpContext.GetOwinContext().Get<RoleManager<IdentityRole, string>>();
        public MainContext Context => HttpContext.GetOwinContext().Get<MainContext>();

        #region Students Administration
        public ActionResult StudentAdministration()
        {
            return View();
        }
        public ActionResult AddStudent()
        {
            return View(new NewStudentModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStudent(NewStudentModel model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if (ModelState.IsValid)
                {

                    CommonAppUser userAccount = new CommonAppUser
                    {
                        FullName = model.FullName,
                        Email = model.Email,
                        UserName = model.StudentID
                    };
                    Class user = Context.Classes.FirstOrDefault(x => x.Id.ToString() == model.ClassID.ToString());
                    userAccount.Class = user;
                    IdentityResult createUserResult = await UserManager.CreateAsync(userAccount, model.Password);
                    if (createUserResult.Succeeded)
                    {
                        UserManager.AddToRole(userAccount.Id, "Student");
                    }
                    else
                    {
                        success = true;
                        errors = errors.Concat(createUserResult.Errors).ToList();
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
                return RedirectToAction("StudentAdministration");
            }
            else if (success && errors.Count > 0)
            {
                
                foreach(string error in errors)
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
            await UserManager.DeleteAsync(user);
            return RedirectToAction("StudentAdministration");
        }
        #endregion


        #region AssignmentAdministration
        public ActionResult AssignmentAdministration()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddNewAssignment(Assignment model)
        {
            bool success = true;
            List<string> errors = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    var prevAssignment = Context.Assignments.FirstOrDefault(x => x.AssignmentName.Equals(model.AssignmentName, StringComparison.InvariantCultureIgnoreCase));
                    if (prevAssignment == null)
                    {
                        Context.Assignments.Add(new Assignment
                        {
                            AssignmentName = model.AssignmentName
                        });
                        int row = await Context.SaveChangesAsync();
                        if (row > 0)
                            ViewBag.Message = "Assignment created successfully";
                        else
                            ViewBag.Message = "Failed when adding assignment to database";
                    }
                    else
                    {
                        ViewBag.Message = "Assignment already exists in database!";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bad Request");
                    success = false;
                }
            }
            catch(Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
                errors.Add(ex.InnerException?.Message);
            }

            if (success && errors.Count == 0)
            {
                model.AssignmentName = "";
                return View("AssignmentAdministration",model);
            }
            foreach (string error in errors)
            {
                ModelState.AddModelError("", error);

            }
            return View("AssignmentAdministration",model);
        }

        
        #endregion

        #region Class Administration
        public ActionResult ClassAdministration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostNewClass(StudentClass model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if(ModelState.IsValid)
                {
                    Context.Classes.Add(new Class
                    {
                        ClassName = model.ClassName
                    });
                    var result = Context.SaveChanges();
                    if (result <= 0)
                    {
                        success = false;
                        errors.Add("An error occured while adding data to database");

                    }
                }
                else
                {
                    success = false;
                    errors.Add("Please fill required fields!");
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors.Add(ex.ToString());
            }
            if(success && errors.Count == 0)
            {
                return RedirectToAction("ClassAdministration");
            }
            else
            {
                string error = errors.Aggregate((current, next) => current + "\n" + next);
                ViewBag.Message = error;
                return View("ClassAdministration", model);
            }
        }
        #endregion


        #region Profile
        public async Task<ActionResult> AdminProfile()
        {
            CommonAppUser commonAppUser =
                await UserManager.FindByIdAsync(
                            User.Identity.GetUserId<string>()
                        );

            return View(new AdminProfile
            {
                Email = commonAppUser.Email
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminProfile(AdminProfile model)
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
        #endregion


    }
}