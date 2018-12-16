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
using System.IO;
using OfficeOpenXml;
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
            foreach(var submittedAssigment in user.SubmittedAssignments.ToList())
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


        public ActionResult ImportStudents()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> BulkAddStudents()
        {
            if (HttpContext.Request.Files.Count > 0)
            {
                try
                {
                    var file = HttpContext.Request.Files["excelFile"];
                    List<StudentImportModel> list = new List<StudentImportModel>();
                    using (Stream stream = file.InputStream)
                    {
                        using (ExcelPackage package = new ExcelPackage())
                        {
                            package.Load(stream);
                            using (ExcelWorkbook workbook = package.Workbook)
                            {
                                using (ExcelWorksheet worksheet = workbook.Worksheets.FirstOrDefault())
                                {
                                    var maxRow = worksheet.Cells.Where(x => x.Value != null).Last().End.Row;
                                    var maxCol = 5;
                                    var sheet = worksheet.Cells[2, 1, maxRow, maxCol];
                                    for (int row = 2; row <= maxRow; row++)
                                    {
                                        StudentImportModel model = new StudentImportModel
                                        {
                                            UserName = sheet[row, 1].GetValue<string>(),
                                            Password = sheet[row, 2].GetValue<string>(),
                                            FullName = sheet[row, 3].GetValue<string>(),
                                            Email = sheet[row, 4].GetValue<string>(),
                                            Class = sheet[row, 5].GetValue<string>()
                                        };
                                        list.Add(model);

                                    }
                                }
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        var role = await RoleManager.FindByNameAsync("Student");
                        if (role == null)
                        {
                            await RoleManager.CreateAsync(new IdentityRole("Student"));
                            role = await RoleManager.FindByNameAsync("Student");
                        }
                        foreach (var item in list)
                        {
                            try
                            {
                                if(Context
                                    .Users
                                    .FirstOrDefault(x => x.Roles.Any(y => y.RoleId == role.Id) &&
                                        x.UserName.Equals(item.UserName,StringComparison.InvariantCultureIgnoreCase) && 
                                        x.Class.ClassName.Equals(item.Class,StringComparison.InvariantCultureIgnoreCase))==null)
                                {
                                    CommonAppUser student = new CommonAppUser
                                    {
                                        FullName = item.FullName,
                                        UserName = item.UserName,
                                        Email = item.Email
                                    };
                                    var @class = Context.Classes.FirstOrDefault(x => x.ClassName.Equals(item.Class, StringComparison.InvariantCultureIgnoreCase));
                                    if (@class == null)
                                    {
                                        @class = new Class
                                        {
                                            ClassName = item.Class
                                        };
                                        Context.Classes.Add(@class);
                                        Context.SaveChanges();
                                    }
                                    student.Class = @class;
                                    var createResult =  await UserManager.CreateAsync(student, item.Password);
                                    if (createResult.Succeeded)
                                    {
                                        UserManager.AddToRole(student.Id, "Student");
                                    }
                                    
                                }
                                    
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return RedirectToAction("StudentAdministration");
        }
        #endregion

        #region Teacher Administration
        public ActionResult TeacherAdministration()
        {
            return View();
        }
        public ActionResult AddNewTeacher()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewTeacher(TeacherModel model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            string successId = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null && !string.IsNullOrEmpty(model.FullName) &&
                    !string.IsNullOrEmpty(model.Email) &&
                    !string.IsNullOrEmpty(model.Password) &&
                    !string.IsNullOrEmpty(model.TeacherID))
                    {
                        if (!RoleManager.RoleExists("Teacher"))
                        {
                            RoleManager.Create(new IdentityRole("Teacher"));
                        }
                        CommonAppUser teacher = new CommonAppUser
                        {
                            FullName = model.FullName,
                            Email = model.Email,
                            UserName = model.TeacherID
                        };

                        var result = await UserManager.CreateAsync(teacher, model.Password);
                        if (result.Succeeded)
                        {
                            await UserManager.AddToRoleAsync(teacher.Id, "Teacher");
                            successId = teacher.Id;
                        }
                        else
                        {
                            errors.AddRange(result.Errors);
                        }
                    }
                    else
                    {

                        errors.Add("Please complete all fields");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please complete all fields");
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                success = false;
            }

            if (success && errors.Count == 0)
            {
                return RedirectToAction("TeacherDetail", new { id=successId });
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

        public async Task<ActionResult> TeacherDetail(string id)
        {
            var teacher = await UserManager.FindByIdAsync(id);
            TeacherModel teacherModel = new TeacherModel()
            {
                FullName = teacher.FullName,
                Email = teacher.Email,
                Id = Guid.Parse(teacher.Id)
            };
            return View(teacherModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> TeacherDetail(TeacherModel model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if (model != null && !string.IsNullOrEmpty(model.FullName) &&
                     !string.IsNullOrEmpty(model.Email))
                {



                    CommonAppUser teacher = Context.Users.FirstOrDefault(x=>x.Id == model.Id.ToString());
                    teacher.FullName = model.FullName;
                    teacher.Email = model.Email;


                    var result = await UserManager.UpdateAsync(teacher);

                    if (!result.Succeeded)
                    {
                        errors.AddRange(result.Errors);
                    }

                }
                else
                {

                    errors.Add("Please complete all fields");
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                success = false;
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
        public async Task<ActionResult> DeleteTeacher(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new CustomException
                {
                    Errors = new List<string>
                    {
                        "Teacher not found"
                    }
                };
            try
            {
                var teacher = Context.Users.FirstOrDefault(x => x.Id.ToString() == id);
                if (teacher == null)
                {
                    throw new CustomException
                    {
                        Errors = new List<string>
                        {
                            "Teacher not found"
                        }
                    };
                }
                foreach(var cls in teacher.TeacherClasses.ToList())
                {
                    Context.TeacherClasses.Remove(cls);
                }
                Context.Users.Remove(teacher);
                await Context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new CustomException
                {
                    Errors = new List<string> { ex.Message }
                };
            }
            
            return RedirectToAction("TeacherAdministration");
        }
        #endregion

        #region AssignmentAdministration
        public ActionResult AssignmentAdministration()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddNewAssignment(AssignmentModel model)
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
        [AllowAnonymous]
        public ActionResult ClassAdministration()
        {

            ViewBag.Message = HttpContext.Session["message"] != null ? HttpContext.Session["message"] : "";
            HttpContext.Session["message"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult PostNewClass(StudentClass model)
        {
            HttpContext.Session["message"] = "";
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if(ModelState.IsValid)
                {
                    var existing = Context.Classes.FirstOrDefault(x => x.ClassName.Equals(model.ClassName, StringComparison.InvariantCultureIgnoreCase));
                    if (existing != null)
                    {
                        HttpContext.Session["message"]= "Class already exists. Cannot add duplicate class!";
                    }
                    else
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
                HttpContext.Session["message"] = error;
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