namespace PlagiarismCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeacherColummToSubmittedAssignment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeacherClasses", "StudentClass_Id", "dbo.StudentClasses");
            AddColumn("dbo.SubmittedAssignments", "Teacher", c => c.String());
            AddForeignKey("dbo.TeacherClasses", "StudentClass_Id", "dbo.StudentClasses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeacherClasses", "StudentClass_Id", "dbo.StudentClasses");
            DropColumn("dbo.SubmittedAssignments", "Teacher");
            AddForeignKey("dbo.TeacherClasses", "StudentClass_Id", "dbo.StudentClasses", "Id", cascadeDelete: true);
        }
    }
}
