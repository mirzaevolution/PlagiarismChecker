namespace PlagiarismCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubmittedAssignmentReferenceToClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubmittedAssignments", "Class_Id", c => c.Guid());
            CreateIndex("dbo.SubmittedAssignments", "Class_Id");
            AddForeignKey("dbo.SubmittedAssignments", "Class_Id", "dbo.StudentClasses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubmittedAssignments", "Class_Id", "dbo.StudentClasses");
            DropIndex("dbo.SubmittedAssignments", new[] { "Class_Id" });
            DropColumn("dbo.SubmittedAssignments", "Class_Id");
        }
    }
}
