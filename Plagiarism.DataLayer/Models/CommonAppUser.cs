using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Plagiarism.DataLayer.Models
{
    public class CommonAppUser: IdentityUser
    {
        public CommonAppUser()
        {
            Assignments = new List<Assignment>();
            SubmittedAssignments = new List<SubmittedAssignment>();
        }
        public string FullName { get; set; }
        public Guid? ClassID { get; set; }
        public virtual Class Class { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<SubmittedAssignment> SubmittedAssignments { get; set; }

    }
}
