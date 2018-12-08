namespace PlagiarismCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTwoFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubmittedAssignments", "Title", c => c.String());
            AddColumn("dbo.SubmittedAssignments", "Score", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubmittedAssignments", "Score");
            DropColumn("dbo.SubmittedAssignments", "Title");
        }
    }
}
