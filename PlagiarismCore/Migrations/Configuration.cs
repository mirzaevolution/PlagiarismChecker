namespace PlagiarismCore.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Plagiarism.DataLayer.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<Plagiarism.DataLayer.Models.MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Plagiarism.DataLayer.Models.MainContext context)
        {
           
            Class[] classes = new Class[]
            {
               new Class
               {
                   ClassName = "X-A"
               },
               new Class
               {
                   ClassName = "X-B"
               },
               new Class
               {
                   ClassName = "X-C"
               },
               new Class
               {
                   ClassName = "XI-A"
               },
               new Class
               {
                   ClassName = "XI-B"
               },
               new Class
               {
                   ClassName = "XI-C"
               },
               new Class
               {
                   ClassName = "XII-A"
               },
               new Class
               {
                   ClassName = "XII-B"
               },
               new Class
               {
                   ClassName = "XII-C"
               }
            };
            context.Classes.AddRange(classes);
            Assignment assignment = new Assignment
            {
                AssignmentName = "English Composition 1"
            };
            context.Assignments.Add(assignment);
            CommonAppUser admin = new CommonAppUser
            {
                UserName = "Admin",
                FullName = "System Administrator",
                Email = "ghulamcyber@hotmail.com"

            };
            context.SaveChanges();

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);
            roleManager.Create(new IdentityRole
            {
                Name = "Admin"
            });
            roleManager.Create(new IdentityRole
            {
                Name = "Student"
            });
            UserStore<CommonAppUser> userStore = new UserStore<CommonAppUser>(context);
            UserManager<CommonAppUser> userManager = new UserManager<CommonAppUser>(userStore);
            userManager.Create(admin, "future");
            userManager.AddToRole(admin.Id, "Admin");
        }
    }
}
