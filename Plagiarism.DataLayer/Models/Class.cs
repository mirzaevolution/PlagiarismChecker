using System;
using System.Collections.Generic;

namespace Plagiarism.DataLayer.Models
{
    public class Class
    {
        public Class()
        {
            CommonAppUsers = new List<CommonAppUser>();
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClassName { get; set; }
        public virtual ICollection<CommonAppUser> CommonAppUsers { get; set; }
    }
}
