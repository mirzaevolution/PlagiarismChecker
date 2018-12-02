using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism.DataLayer.Models
{
    public class SubmittedAssignment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UploadedFilePath { get; set; }
        public int PercentageInteger { get; set; }
        public float Percentage { get; set; }
        public string Description { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; } = false;
        public string Data { get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual ICollection<CommonAppUser> CommonAppUsers { get; set; }
    }
}
