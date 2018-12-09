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
using System.IO;
using Plagiarism.CoreLibrary.Models;
using Plagiarism.CoreLibrary.Libraries;
namespace PlagiarismCore.Controllers
{
    [Authorize(Roles = "Student")]
    public class ManageUserController : Controller
    {
        public UserManager<CommonAppUser> UserManager => HttpContext.GetOwinContext().GetUserManager<UserManager<CommonAppUser>>();
        public SignInManager<CommonAppUser, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<CommonAppUser, string>>();
        public RoleManager<IdentityRole, string> RoleManager => HttpContext.GetOwinContext().Get<RoleManager<IdentityRole, string>>();
        public MainContext Context => HttpContext.GetOwinContext().Get<MainContext>();

        // GET: ManageUser
        public ActionResult Index()
        {
            try
            {
                ViewBag.UserID = SignInManager.AuthenticationManager.User.Identity.GetUserId<string>();
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex);
                throw new CustomException
                {
                    Errors = new List<string>()
                    {
                        ex.ToString()
                    }
                };
            }
            return View();
        }

        public ActionResult AddSubmission()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubmission(SubmissionModel model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if (ModelState.IsValid)
                {
                    if(Request.Files.Count>0)
                    {
                        Assignment assignment = Context.Assignments.FirstOrDefault(x => x.Id.ToString() == model.AssignmentId);
                        if (assignment != null)
                        {
                            HttpPostedFileBase uploadedFile = Request.Files["AssignmentFile"];
                            string data;
                            using (Stream stream = uploadedFile.InputStream)
                            {
                                data = PDFReader.ExtractTextFromPdf(stream);
                            }
                            string randomFileName = Path.GetFileName(uploadedFile.FileName) + "_"
                                + DateTime.Now.Ticks.ToString() + Path.GetExtension(uploadedFile.FileName);
                            string relativePath = "~/Upload/" + randomFileName;
                            string path = Path.Combine(Server.MapPath("~/Upload"), randomFileName);
                            uploadedFile.SaveAs(path);
                            model.UploadedFilePath = relativePath;
                            model.Data = data;
                            int? latestCounter =
                                Context.SubmittedAssignments.Where(x => x.Assignment.Id == assignment.Id)
                                .OrderBy(x => x.Counter)
                                .Select(x=>x.Counter)
                                .ToList()
                                .LastOrDefault();
                            if (latestCounter!=null)
                            {
                                model.Rank = latestCounter.Value + 1;
                                model.UploadedTime = DateTime.Now;
                                CommonAppUser commonAppUser =
                                    await UserManager.FindByIdAsync(
                                                User.Identity.GetUserId<string>()
                                            );
                                commonAppUser.SubmittedAssignments.Add(new SubmittedAssignment
                                {
                                    Assignment = assignment,
                                    Counter = model.Rank,
                                    Data = model.Data,
                                    SubmissionDate = model.UploadedTime,
                                    UploadedFilePath = model.UploadedFilePath,
                                    Title = model.Title
                                });
                                var result = await UserManager.UpdateAsync(commonAppUser);
                                if(!result.Succeeded)
                                {
                                    errors.AddRange(result.Errors);
                                }
                            }
                            else
                            {
                                success = false;
                                errors.Add("Cannot retrieve latest submitted rank from database");
                            }
                        }
                        else
                        {
                            success = false;
                            errors.Add("Assignment selected is not found");
                        }
                      
                        
                    }
                    else
                    {
                        success = false;
                        errors.Add("Please upload required document");
                    }
                    
                }
                else
                {
                    success = false;
                    errors.Add("Please complete all required fields!");
                }
            }
            catch(Exception ex)
            {
                success = false;
                errors.Add(ex.ToString());
            }
            if(success && errors.Count==0)
            {
                return View("WaitingForQuery");
            }
            foreach (string error in errors)
            {
                ModelState.AddModelError("", error);

            }
            return View(model);
        }

    

        public async Task<ActionResult> MyProfile()
        {
            CommonAppUser commonAppUser =
                await UserManager.FindByIdAsync(
                            User.Identity.GetUserId<string>()
                        );

            return View(new StudentProfile
            {
                FullName = commonAppUser.FullName,
                Email = commonAppUser.Email
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MyProfile(StudentProfile model)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                if(ModelState.IsValid)
                {
                    CommonAppUser commonAppUser =
                                    await UserManager.FindByIdAsync(
                                                User.Identity.GetUserId<string>()
                                            );
                    commonAppUser.FullName = model.FullName;
                    commonAppUser.Email = model.Email;
                    var updateResult  =  await UserManager.UpdateAsync(commonAppUser);

                    if (updateResult.Succeeded)
                    {
                        if( !string.IsNullOrEmpty(model.NewPassword) && 
                            !string.IsNullOrEmpty(model.CurrentPassword))
                        {
                            updateResult = await UserManager.ChangePasswordAsync(commonAppUser.Id,
                                    model.CurrentPassword, model.NewPassword
                                );
                            if(!updateResult.Succeeded)
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
            catch(Exception ex)
            {
                success = false;
                errors.Add(ex.ToString());
            }
            model.CurrentPassword = "";
            model.NewPassword = "";
            if(success && errors.Count == 0)
            {
                return View(model);
            }
            else
            {
                foreach(string error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }
        }
    }
}