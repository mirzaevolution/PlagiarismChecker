using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlagiarismCore.Models.IdentityModels
{
    public class SubmittedAssignmentList
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentClass { get; set; }
        public string SubjectName { get; set; }
        public string Title { get; set; }
        public int Counter { get; set; }
        public string UploadedFilePath { get; set; }
        public int PercentageInteger { get; set; }
        public string Description { get; set; }
        public string IsChecked { get; set; } 
        public short Score { get; set; }
        public string ScoreStatus { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }

    }
}