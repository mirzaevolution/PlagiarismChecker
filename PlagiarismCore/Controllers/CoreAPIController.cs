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
    [Authorize]
    public class CoreAPIController : Controller
    {
        public UserManager<CommonAppUser> UserManager => HttpContext.GetOwinContext().GetUserManager<UserManager<CommonAppUser>>();
        public SignInManager<CommonAppUser, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<CommonAppUser, string>>();
        public RoleManager<IdentityRole, string> RoleManager => HttpContext.GetOwinContext().Get<RoleManager<IdentityRole, string>>();
        public MainContext Context => HttpContext.GetOwinContext().Get<MainContext>();


        #region API
        private string GetStatus(bool isAccepted, bool isChecked)
        {
            if (!isAccepted && !isChecked)
                return "Under Review";
            else if (isChecked && isChecked)
                return "Accepted";
            return "Rejected";
        }
        public JsonResult GetClasses()
        {
            var classes = Context.Classes.Select(x => new
            {
                ID = x.Id,
                Text = x.ClassName
            }).OrderBy(x => x.Text).ToList();
            return Json(classes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllAssignmentsList()
        {
            var assignments = Context.Assignments.Select(x => new
            {
                ID = x.Id,
                Text = x.AssignmentName
            }).OrderBy(x => x.Text).ToList();
            return Json(assignments, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllClasses()
        {
            var classes = Context.Classes.Select(x => new
            {
                x.Id,
                x.ClassName,
                TotalStudents = x.CommonAppUsers.Count
            }).OrderBy(x => x.ClassName).ToList();
            return Json(new { data=classes }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllStudentClasses(string id)
        {
            List<StudentModel> list = new List<StudentModel>();
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { data = list }, JsonRequestBehavior.AllowGet);
            }
            var @class =  Context.Classes.FirstOrDefault(x => x.Id.ToString() == id);
            if(@class==null)
                return Json(new { data = list }, JsonRequestBehavior.AllowGet);
            var role = RoleManager.FindByName("Student");
            list = @class
                .CommonAppUsers
                .Where(x => x.Roles.Any(y => y.RoleId == role.Id))
                .Select(x => new StudentModel
                {
                    FullName = x.FullName,
                    Email = x.Email,
                    StudentID = x.UserName
                }).ToList();
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);

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
                    x.FullName,
                     x.Email,
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
                    
                    x.Id,
                    AssignmentName = x.Assignment!=null?x.Assignment.AssignmentName:"Unknown",
                    x.Title,
                    x.Description,
                    x.PercentageInteger,
                    Status = GetStatus(x.IsAccepted,x.IsChecked),
                    x.SubmissionDate,
                    UploadedFilePath=x.UploadedFilePath.Replace("~",""),
                    x.Score,
                    x.Counter,
                    IsChecked= x.IsChecked? "Checked":"In Review"
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
        public JsonResult GetAllSubmittedAssignments()
        {
            List<SubmittedAssignmentList> list = new List<SubmittedAssignmentList>();
            var role = RoleManager.FindByName("Student");
            foreach (var submittedAssignment in Context.SubmittedAssignments.ToList())
            {
                SubmittedAssignmentList item = new SubmittedAssignmentList
                {
                    SubjectName = submittedAssignment.Assignment == null ?
                                   "Undefined" : submittedAssignment.Assignment.AssignmentName,
                    Title = submittedAssignment.Title,
                    Counter = submittedAssignment.Counter,
                    UploadedFilePath = submittedAssignment.UploadedFilePath,
                    PercentageInteger = submittedAssignment.PercentageInteger,
                    Description = submittedAssignment.Description,
                    Status = GetStatus(submittedAssignment.IsAccepted, submittedAssignment.IsChecked),
                    Score = submittedAssignment.Score,
                    IsChecked = submittedAssignment.IsChecked?"Checked":"Not yet",
                    SubmissionDate = submittedAssignment.SubmissionDate
                };
                foreach(var student in submittedAssignment
                    .CommonAppUsers
                    .Where(x => x.Roles.Any(y => y.RoleId == role.Id))
                    .ToList())
                {
                    item.StudentName = student.FullName;
                    item.StudentClass = student.Class?.ClassName;
                    list.Add(item);
                }
            }
            return Json(new { data = list },JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EditStudentAssignmentScore(string assignmentId, short score)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if(score<=1 || score > 100)
                {
                    success = false;
                    errors.Add("Given score is invalid");
                }
                else
                {
                    var submittedAssignment = Context.SubmittedAssignments.Find(Guid.Parse(assignmentId));
                    if (submittedAssignment != null)
                    {
                        submittedAssignment.Score = score;
                        int result = Context.SaveChanges();
                        if (result <= 0)
                        {
                            success = false;
                            errors.Add("Failed when updating user score");
                        }
                    }
                    else
                    {
                        success = false;
                        errors.Add("Assignment not found");
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
        public JsonResult EditAssignment(Assignment model)
        {
            bool success = true;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(model.AssignmentName))
            {
                success = false;
                errors.Add("Assignment name cannot be empty");
            }
            try
            {
                if (success)
                {
                    var assignment = Context.Assignments.Find(model.Id);
                    if (assignment != null)
                    {
                        assignment.AssignmentName = model.AssignmentName;
                        int i = Context.SaveChanges();
                        if (i <= 0)
                        {
                            success = false;
                            errors.Add("Error while saving data to database");
                        }
                    }
                    else
                    {
                        errors.Add("Assignment not found");
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
        [HttpPost]
        public JsonResult DeleteAssignment(string id)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var assignment = Context.Assignments.FirstOrDefault(x => x.Id.ToString() == id);
                    if (assignment != null)
                    {
                        Context.Assignments.Remove(assignment);
                        Context.SaveChanges();
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
                    errors.Add("Id parameter cannot be null or empty");
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
        public JsonResult DeleteUserAssignment(string userId, string assignmentId)
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

        [HttpPost]
        public JsonResult EditClass(StudentClass model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if (ModelState.IsValid && !string.IsNullOrEmpty(model.ClassName))
                {
                    var @class = Context.Classes.FirstOrDefault(x => x.Id.ToString() == model.Id.ToString());

                    if(@class!=null)
                    {
                        @class.ClassName = model.ClassName;
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
                        errors.Add("Class not found");
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
            return Json(new { Success = success, Errors = errors });
        }

        [HttpPost]
        public JsonResult DeleteClass(string id)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                var @class = Context.Classes.FirstOrDefault(x => x.Id.ToString() == id);

                if (@class != null)
                {
                    Context.Classes.Remove(@class);
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
                    errors.Add("Class not found");
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