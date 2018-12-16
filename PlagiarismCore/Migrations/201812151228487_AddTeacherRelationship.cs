namespace PlagiarismCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeacherRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeacherClasses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClassName = c.String(nullable: false, maxLength: 256, unicode: false),
                        StudentClass_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudentClasses", t => t.StudentClass_Id, cascadeDelete: true)
                .Index(t => t.StudentClass_Id);
            
            CreateTable(
                "dbo.UserTeacherTeacherClasses",
                c => new
                    {
                        TeacherId = c.String(nullable: false, maxLength: 128),
                        TeacherClass = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeacherId, t.TeacherClass })
                .ForeignKey("dbo.AspNetUsers", t => t.TeacherId, cascadeDelete: false)
                .ForeignKey("dbo.TeacherClasses", t => t.TeacherClass, cascadeDelete: false)
                .Index(t => t.TeacherId)
                .Index(t => t.TeacherClass);
            
            AddColumn("dbo.SubmittedAssignments", "TeacherClass_Id", c => c.Guid());
            CreateIndex("dbo.SubmittedAssignments", "TeacherClass_Id");
            AddForeignKey("dbo.SubmittedAssignments", "TeacherClass_Id", "dbo.TeacherClasses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTeacherTeacherClasses", "TeacherClass", "dbo.TeacherClasses");
            DropForeignKey("dbo.UserTeacherTeacherClasses", "TeacherId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubmittedAssignments", "TeacherClass_Id", "dbo.TeacherClasses");
            DropForeignKey("dbo.TeacherClasses", "StudentClass_Id", "dbo.StudentClasses");
            DropIndex("dbo.UserTeacherTeacherClasses", new[] { "TeacherClass" });
            DropIndex("dbo.UserTeacherTeacherClasses", new[] { "TeacherId" });
            DropIndex("dbo.TeacherClasses", new[] { "StudentClass_Id" });
            DropIndex("dbo.SubmittedAssignments", new[] { "TeacherClass_Id" });
            DropColumn("dbo.SubmittedAssignments", "TeacherClass_Id");
            DropTable("dbo.UserTeacherTeacherClasses");
            DropTable("dbo.TeacherClasses");
        }
    }
}
