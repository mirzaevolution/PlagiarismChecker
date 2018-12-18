using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlagiarismCore.Models.IdentityModels
{
    public class TeacherModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }

        [Display(Name = "Teacher ID")]
        [Required]
        public string TeacherID { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
    public class TeacherClassModel
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
    }
}