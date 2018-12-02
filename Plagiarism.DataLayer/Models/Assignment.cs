using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism.DataLayer.Models
{
    public class Assignment
    {
        public Assignment()
        {
            CommonAppUsers = new List<CommonAppUser>();
            SubmittedAssignments = new List<SubmittedAssignment>();
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string AssignmentName { get; set; }
        public ICollection<CommonAppUser> CommonAppUsers { get; set; }
        public ICollection<SubmittedAssignment> SubmittedAssignments { get; set; }
    }
}
