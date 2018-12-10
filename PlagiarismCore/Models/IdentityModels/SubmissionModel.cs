using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlagiarismCore.Models.IdentityModels
{
    public class SubmissionModel
    {
        public string StudentId { get; set; }
        [Required]
        public string AssignmentId { get; set; }
        public string Title { get; set; }
        public DateTime UploadedTime { get; set; }
        public string UploadedFilePath { get; set; }
        public string Data { get; set; }
        public int Rank { get; set; }
    }
}