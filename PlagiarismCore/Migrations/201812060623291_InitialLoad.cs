namespace PlagiarismCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialLoad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AssignmentName = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 256, unicode: false),
                        ClassID = c.Guid(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudentClasses", t => t.ClassID, cascadeDelete: true)
                .Index(t => t.ClassID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StudentClasses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClassName = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SubmittedAssignments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Counter = c.Int(nullable: false),
                        UploadedFilePath = c.String(unicode: false),
                        PercentageInteger = c.Int(nullable: false),
                        Percentage = c.Single(nullable: false),
                        Description = c.String(),
                        IsAccepted = c.Boolean(nullable: false),
                        IsChecked = c.Boolean(nullable: false),
                        Data = c.String(),
                        Assignment_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignments", t => t.Assignment_Id, cascadeDelete: true)
                .Index(t => t.Assignment_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserAssignments",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        AssignmentId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.AssignmentId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Assignments", t => t.AssignmentId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.SubmittedAssignmentCommonAppUsers",
                c => new
                    {
                        SubmittedAssignment_Id = c.Guid(nullable: false),
                        CommonAppUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SubmittedAssignment_Id, t.CommonAppUser_Id })
                .ForeignKey("dbo.SubmittedAssignments", t => t.SubmittedAssignment_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CommonAppUser_Id, cascadeDelete: true)
                .Index(t => t.SubmittedAssignment_Id)
                .Index(t => t.CommonAppUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.SubmittedAssignmentCommonAppUsers", "CommonAppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubmittedAssignmentCommonAppUsers", "SubmittedAssignment_Id", "dbo.SubmittedAssignments");
            DropForeignKey("dbo.SubmittedAssignments", "Assignment_Id", "dbo.Assignments");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ClassID", "dbo.StudentClasses");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserAssignments", "AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.UserAssignments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SubmittedAssignmentCommonAppUsers", new[] { "CommonAppUser_Id" });
            DropIndex("dbo.SubmittedAssignmentCommonAppUsers", new[] { "SubmittedAssignment_Id" });
            DropIndex("dbo.UserAssignments", new[] { "AssignmentId" });
            DropIndex("dbo.UserAssignments", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SubmittedAssignments", new[] { "Assignment_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "ClassID" });
            DropTable("dbo.SubmittedAssignmentCommonAppUsers");
            DropTable("dbo.UserAssignments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SubmittedAssignments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.StudentClasses");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Assignments");
        }
    }
}
