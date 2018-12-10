namespace PlagiarismCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNoteColumnToSubmittedAssignment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubmittedAssignments", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubmittedAssignments", "Note");
        }
    }
}
