using System;
using System.Collections.Generic;

namespace Plagiarism.DataLayer.Models
{
    public class Class
    {
        public Class()
        {
            CommonAppUsers = new List<CommonAppUser>();
            SubmittedAssignments = new List<SubmittedAssignment>();
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClassName { get; set; }
        public virtual ICollection<CommonAppUser> CommonAppUsers { get; set; }
        public virtual ICollection<SubmittedAssignment> SubmittedAssignments { get; set; }
        public virtual TeacherClass TeacherClass { get; set; }
    }

    public class TeacherClass
    {

        public TeacherClass()
        {
            Teachers = new List<CommonAppUser>();
            SubmittedAssignments = new List<SubmittedAssignment>();
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClassName { get; set; }
        public virtual ICollection<CommonAppUser> Teachers { get; set; }
        public virtual ICollection<SubmittedAssignment> SubmittedAssignments { get; set; }
        public virtual Class StudentClass { get; set; }
    }
}
