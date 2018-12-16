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
using System.Diagnostics;

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
            else if (isAccepted && isChecked)
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
            return Json(new { data = classes }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllStudentClasses(string id)
        {
            List<StudentModel> list = new List<StudentModel>();
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { data = list }, JsonRequestBehavior.AllowGet);
            }
            var @class = Context.Classes.FirstOrDefault(x => x.Id.ToString() == id);
            if (@class == null)
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
        public JsonResult GetAssignments(string id, bool except = true)
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
            if (!except)
            {
                return Json(userAssignments, JsonRequestBehavior.AllowGet);
            }
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


        public JsonResult GetAllTeachers()
        {
            var teacherRole = RoleManager.FindByName("Teacher");
            if (teacherRole != null)
            {
                var teachers = Context
                    .Users
                    .Where(x => x.Roles.Any(r => r.RoleId == teacherRole.Id)).Select(x => new
                    {
                        x.Id,
                        x.FullName,
                        TeacherId = x.UserName,
                        x.Email,
                        TotalClasses = x.TeacherClasses.Count,
                        TotalSubjects = x.Assignments.Count
                    });
                return Json(new { data = teachers }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = new List<string>() }, JsonRequestBehavior.AllowGet);
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
                    AssignmentName = x.Assignment != null ? x.Assignment.AssignmentName : "Unknown",
                    x.Title,
                    x.Description,
                    x.PercentageInteger,
                    Status = GetStatus(x.IsAccepted, x.IsChecked),
                    x.SubmissionDate,
                    UploadedFilePath = x.UploadedFilePath.Replace("~", ""),
                    x.Score,
                    ScoreStatus = x.Score == 0 ? "Waiting" : "Done",
                    x.Counter,
                    IsChecked = x.IsChecked ? "Checked" : "In Review",
                    Note = string.IsNullOrEmpty(x.Note) ? "" : x.Note
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
            return Json(new { data = assignments }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllSubmittedAssignments()
        {
            List<SubmittedAssignmentList> list = new List<SubmittedAssignmentList>();
            var role = RoleManager.FindByName("Student");
            try
            {
                foreach (var submittedAssignment in Context.SubmittedAssignments.ToList())
                {
                    SubmittedAssignmentList item = new SubmittedAssignmentList
                    {

                        SubjectName = submittedAssignment.Assignment == null ?
                                       "Undefined" : submittedAssignment.Assignment.AssignmentName,
                        Title = submittedAssignment.Title,
                        Counter = submittedAssignment.Counter,
                        UploadedFilePath = submittedAssignment.UploadedFilePath.Replace("~", ""),
                        PercentageInteger = submittedAssignment.PercentageInteger,
                        Description = submittedAssignment.Description,
                        Status = GetStatus(submittedAssignment.IsAccepted, submittedAssignment.IsChecked),
                        Score = submittedAssignment.Score,
                        ScoreStatus = submittedAssignment.Score == 0 ? "Waiting" : "Done",
                        IsChecked = submittedAssignment.IsChecked ? "Checked" : "Not yet",
                        SubmissionDate = submittedAssignment.SubmissionDate,
                        Note = string.IsNullOrEmpty(submittedAssignment.Note) ? "" : submittedAssignment.Note
                    };
                    foreach (var student in submittedAssignment
                        .CommonAppUsers
                        .Where(x => x.Roles.Any(y => y.RoleId == role.Id))
                        .ToList())
                    {
                        item.StudentId = student.Id;
                        item.StudentName = student.FullName;
                        item.StudentClass = student.Class?.ClassName;
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                list.Clear();
                Trace.WriteLine(ex);
            }
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailableTeacherClasses(string teacherId)
        {
            if (string.IsNullOrEmpty(teacherId))
            {
                return Json(new { data = new List<TeacherClassModel>() }, JsonRequestBehavior.AllowGet);
            }
            var allClasses = Context.Classes.Select(x => new
            {
                x.Id,
                x.ClassName
            }).ToList();
            var currentClasses = UserManager.FindById(teacherId).TeacherClasses.Select(x => new
            {
                x.Id,
                x.ClassName
            }).ToList();
            var availabelClasses = allClasses
                .Where(x => !currentClasses.Any(y => y.ClassName.Equals(x.ClassName, StringComparison.InvariantCultureIgnoreCase)))
                .Select(x => new TeacherClassModel
                {
                    ClassName = x.ClassName,
                    Id = x.Id
                }).OrderBy(x => x.ClassName).ToList();
            return Json(new { data = availabelClasses }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetTeacherClasses(string teacherId)
        {

            if (string.IsNullOrEmpty(teacherId))
            {
                return Json(new { data = new List<TeacherClassModel>() }, JsonRequestBehavior.AllowGet);
            }

            var currentClasses = UserManager.FindById(teacherId).TeacherClasses.Select(x => new
            {
                x.Id,
                x.ClassName
            }).ToList();
            return Json(new { data = currentClasses }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAvailableTeacherSubjects(string teacherId)
        {
            if (string.IsNullOrEmpty(teacherId))
            {
                return Json(new { data = new List<AssignmentModel>() }, JsonRequestBehavior.AllowGet);
            }
            var role = RoleManager.FindByName("Teacher");
            if (role != null)
            {
                var teacher = Context
                    .Users
                    .FirstOrDefault(x =>
                            x.Id.Equals(teacherId, StringComparison.InvariantCultureIgnoreCase) &&
                           x.Roles.Any(y => y.RoleId == role.Id));
                if (teacher != null)
                {
                    var subjects = teacher.Assignments.Select(x => new AssignmentModel
                    {
                        AssignmentName = x.AssignmentName,
                        Id = x.Id.ToString()
                    }).ToList();
                    var allSubjects = Context.Assignments.Select(x => new AssignmentModel
                    {
                        Id = x.Id.ToString(),
                        AssignmentName = x.AssignmentName
                    }).ToList();
                    var availableSubjects = allSubjects.
                        Where(x => !subjects.Any(y => y.AssignmentName.Equals(x.AssignmentName, StringComparison.InvariantCultureIgnoreCase)))
                        .OrderBy(x => x.AssignmentName).ToList();
                    return Json(new { data = availableSubjects }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { data = new List<AssignmentModel>() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTeacherSubjects(string teacherId)
        {
            if (string.IsNullOrEmpty(teacherId))
            {
                return Json(new { data = new List<AssignmentModel>() }, JsonRequestBehavior.AllowGet);
            }
            var role = RoleManager.FindByName("Teacher");
            if (role != null)
            {
                var user = Context
                    .Users
                    .FirstOrDefault(x =>
                            x.Id.Equals(teacherId, StringComparison.InvariantCultureIgnoreCase) &&
                           x.Roles.Any(y => y.RoleId == role.Id));
                if (user != null)
                {
                    var subjects = user.Assignments.Select(x => new AssignmentModel
                    {
                        AssignmentName = x.AssignmentName,
                        Id = x.Id.ToString()
                    }).ToList();
                    return Json(new { data = subjects }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { data = new List<AssignmentModel>() }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult EditStudentAssignmentScore(string assignmentId, short score)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if (score <= 1 || score > 100)
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
            catch (Exception ex)
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
            catch (Exception ex)
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

                    if (@class != null)
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




        [HttpPost]
        public JsonResult PostTeacherClass(string userId, string classId)
        {
            bool success = true;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(classId))
            {
                success = false;
                errors.Add("Missing required parameter(s)");
            }
            try
            {
                if (success)
                {
                    var teacher = UserManager.FindById(userId);
                    if (teacher != null)
                    {
                        var teacherClass = Context.Classes.Find(Guid.Parse(classId));
                        if (teacherClass != null)
                        {
                            teacher.TeacherClasses.Add(new TeacherClass
                            {
                                ClassName = teacherClass.ClassName,
                                StudentClass = teacherClass
                            });
                            var result = UserManager.Update(teacher);
                            if (!result.Succeeded)
                            {
                                success = false;
                                errors.AddRange(result.Errors);
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
                        errors.Add("Teacher not found");
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
        public JsonResult DeleteTeacherClass(string userId, string classId)
        {
            bool success = true;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(classId))
            {
                success = false;
                errors.Add("Missing required parameter(s)");
            }
            try
            {
                if (success)
                {
                    var teacher = UserManager.FindById(userId);
                    if (teacher != null)
                    {
                        var teacherClass = Context.TeacherClasses.Find(Guid.Parse(classId));
                        if (teacherClass != null)
                        {
                            teacher.TeacherClasses.Remove(teacherClass);
                            var result = UserManager.Update(teacher);
                            if (!result.Succeeded)
                            {
                                success = false;
                                errors.AddRange(result.Errors);
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
                        errors.Add("Teacher not found");
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

        public JsonResult PostTeacherSubject(string userId, string classId, string subjectId)
        {
            bool success = true;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(subjectId) || string.IsNullOrEmpty(classId))
            {
                success = false;
                errors.Add("Missing required parameter(s)");
            }
            try
            {
                if (success)
                {
                    var teacher = UserManager.FindById(userId);
                    if (teacher != null)
                    {
                        var subject = Context.Assignments.Find(Guid.Parse(subjectId));
                        if (subject != null)
                        {
                            teacher.Assignments.Add(subject);
                            var result = UserManager.Update(teacher);
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
                        errors.Add("Teacher not found");
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
        public JsonResult DeleteTeacherSubject(string userId, string classId, string subjectId)
        {
            bool success = true;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(subjectId) || string.IsNullOrEmpty(classId))
            {
                success = false;
                errors.Add("Missing required parameter(s)");
            }
            try
            {
                if (success)
                {
                    var teacher = UserManager.FindById(userId);
                    if (teacher != null)
                    {
                        var subject = Context.Assignments.Find(Guid.Parse(subjectId));
                        if (subject != null)
                        {
                            teacher.Assignments.Remove(subject);
                            var result = UserManager.Update(teacher);
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
                        errors.Add("Teacher not found");
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

        public JsonResult GetStudentsByTeacherRole()
        {
            var teacherRole = RoleManager.FindByName("Teacher");
            var teacher = UserManager.FindById(User.Identity.GetUserId());

            string[] teacherClasses = teacher.TeacherClasses.Select(x => x.ClassName).ToArray();


            var studentRole = RoleManager.FindByName("Student");
            var students = UserManager
                .Users
                .Where(x =>
                    x.Roles.Any(y => y.RoleId == studentRole.Id) &&
                    teacherClasses.Any(t => t.Equals(x.Class.ClassName, StringComparison.InvariantCultureIgnoreCase))
                )
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

        public JsonResult GetSubjectAssignedStudents(string classId, string subjectId)
        {
            var teacherRole = RoleManager.FindByName("Teacher");
            var teacher = UserManager.FindById(User.Identity.GetUserId());
            var selectedClass = Context.TeacherClasses.FirstOrDefault(x => x.Id.ToString() == classId) != null ?
                                Context.TeacherClasses.FirstOrDefault(x => x.Id.ToString() == classId).ClassName : "";

            var studentRole = RoleManager.FindByName("Student");
            var subjectName = Context.Assignments.FirstOrDefault(x => x.Id.ToString() == subjectId) != null ?
                              Context.Assignments.FirstOrDefault(x => x.Id.ToString() == subjectId).AssignmentName : "";

            var students = Context
                .Users
                .Where(x =>
                    x.Assignments.Any(y => y.AssignmentName.Equals(subjectName)) &&
                    x.Roles.Any(y => y.RoleId == studentRole.Id) &&
                    x.Class.ClassName.Equals(selectedClass, StringComparison.InvariantCultureIgnoreCase)
                )
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
        public JsonResult GetSubjectUnassignedStudents(string classId, string subjectId)
        {
            var teacherRole = RoleManager.FindByName("Teacher");
            var teacher = UserManager.FindById(User.Identity.GetUserId());
            var selectedClass = Context.TeacherClasses.FirstOrDefault(x => x.Id.ToString() == classId) != null ?
                                Context.TeacherClasses.FirstOrDefault(x => x.Id.ToString() == classId).ClassName : "";

            var studentRole = RoleManager.FindByName("Student");
            var subjectName = Context.Assignments.FirstOrDefault(x => x.Id.ToString() == subjectId) != null ?
                              Context.Assignments.FirstOrDefault(x => x.Id.ToString() == subjectId).AssignmentName : "";

            var students = Context
                .Users
                .Where(x =>
                    !x.Assignments.Any(y => y.AssignmentName.Equals(subjectName)) &&
                    x.Roles.Any(y => y.RoleId == studentRole.Id) &&
                    x.Class.ClassName.Equals(selectedClass, StringComparison.InvariantCultureIgnoreCase)
                )
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

        [HttpPost]
        public JsonResult PostStudentsToSubject(string[] ids, string classId, string subjectId)
        {
            try
            {
                var @class = Context.TeacherClasses.FirstOrDefault(x => x.Id.ToString() == classId) != null ?
                Context.TeacherClasses.FirstOrDefault(x => x.Id.ToString() == classId).ClassName : "";
                var subject = Context.Assignments.FirstOrDefault(x => x.Id.ToString() == subjectId);

                var change = false;
                foreach (string id in ids)
                {
                    var student = Context.Users.FirstOrDefault(x => x.Id.ToString() == id && x.Class.ClassName.Equals(@class));
                    if (student != null)
                    {
                        student.Assignments.Add(subject);
                        change = true;
                    }
                }
                if (change)
                {
                    Context.SaveChanges();
                }
                return Json(new { Success = true });
            }
            catch { return Json(new { Success = false }); }
        }

        public JsonResult GetSubmittedAssignmentsByClass(string classId, string subjectId)
        {
            List<SubmittedAssignmentList> list = new List<SubmittedAssignmentList>();

            try
            {

                var @class = Context.TeacherClasses.FirstOrDefault(x => x.Id.ToString() == classId) != null ?
                Context.TeacherClasses.FirstOrDefault(x => x.Id.ToString() == classId).ClassName : "";
                var subject = Context.Assignments.FirstOrDefault(x => x.Id.ToString() == subjectId);
                var role = RoleManager.FindByName("Student");
                foreach (var submittedAssignment in Context
                    .SubmittedAssignments
                    .Where(x => x.Class
                    .ClassName
                    .Equals(@class, StringComparison.InvariantCultureIgnoreCase)
                         && x.Assignment.AssignmentName.Equals(subject.AssignmentName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList())
                {
                    SubmittedAssignmentList item = new SubmittedAssignmentList
                    {

                        SubjectName = submittedAssignment.Assignment == null ?
                                       "Undefined" : submittedAssignment.Assignment.AssignmentName,
                        Title = submittedAssignment.Title,
                        Counter = submittedAssignment.Counter,
                        UploadedFilePath = submittedAssignment.UploadedFilePath.Replace("~", ""),
                        PercentageInteger = submittedAssignment.PercentageInteger,
                        Description = submittedAssignment.Description,
                        Status = GetStatus(submittedAssignment.IsAccepted, submittedAssignment.IsChecked),
                        Score = submittedAssignment.Score,
                        ScoreStatus = submittedAssignment.Score == 0 ? "Waiting" : "Done",
                        IsChecked = submittedAssignment.IsChecked ? "Checked" : "Not yet",
                        SubmissionDate = submittedAssignment.SubmissionDate,
                        Note = string.IsNullOrEmpty(submittedAssignment.Note) ? "" : submittedAssignment.Note
                    };
                    foreach (var student in submittedAssignment
                        .CommonAppUsers
                        .Where(x => x.Roles.Any(y => y.RoleId == role.Id))
                        .ToList())
                    {
                        item.StudentId = student.Id;
                        item.StudentName = student.FullName;
                        item.StudentClass = student.Class?.ClassName;
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                list.Clear();
            }
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}