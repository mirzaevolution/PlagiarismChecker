using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism.DataLayer.Models
{
    public class MainContext:IdentityDbContext<CommonAppUser>
    {
        public MainContext() : base("name=PlagiarismContext") { }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<SubmittedAssignment> SubmittedAssignments { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var userEntity = modelBuilder.Entity<CommonAppUser>();
            userEntity.Property(x => x.FullName).HasMaxLength(256)
                .IsRequired()
                .IsUnicode(false);
            userEntity.Property(x => x.ClassID).IsOptional();
            userEntity
                .HasOptional(x => x.Class)
                .WithMany(x => x.CommonAppUsers)
                .HasForeignKey(x=>x.ClassID)
                .WillCascadeOnDelete();
            userEntity.HasMany(x => x.Assignments)
                .WithMany(x => x.CommonAppUsers)
                .Map(rel =>
                {
                    rel.MapLeftKey("UserId");
                    rel.MapRightKey("AssignmentId");
                    rel.ToTable("UserAssignments");
                });


            var classEntity = modelBuilder.Entity<Class>();
            classEntity.HasKey(x => x.Id);
            classEntity.Property(x => x.ClassName)
                .HasMaxLength(256)
                .IsRequired()
                .IsUnicode(false);
            classEntity.HasMany(x => x.CommonAppUsers)
                .WithOptional(x => x.Class).HasForeignKey(x=>x.ClassID);
            classEntity.ToTable("StudentClasses");

            var assignmentEntity = modelBuilder.Entity<Assignment>();
            assignmentEntity.HasKey(x => x.Id);
            assignmentEntity.Property(x => x.AssignmentName)
                .HasMaxLength(256)
                .IsRequired()
                .IsUnicode(false);
            assignmentEntity.ToTable("Assignments");
            

            var submittedEntity = modelBuilder.Entity<SubmittedAssignment>();
            submittedEntity.HasKey(x => x.Id);
            submittedEntity.Property(x => x.UploadedFilePath)
                .IsUnicode(false);
            submittedEntity
                .HasRequired(x => x.Assignment)
                .WithMany(x => x.SubmittedAssignments)
                .WillCascadeOnDelete();
            submittedEntity.ToTable("SubmittedAssignments");
        }
    }
}
