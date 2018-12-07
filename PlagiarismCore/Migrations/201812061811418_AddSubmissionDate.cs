namespace PlagiarismCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubmissionDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubmittedAssignments", "SubmissionDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubmittedAssignments", "SubmissionDate");
        }
    }
}
