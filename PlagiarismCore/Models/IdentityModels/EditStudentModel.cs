using System;
using System.ComponentModel.DataAnnotations;
namespace PlagiarismCore.Models.IdentityModels
{
    public class StudentModel
    {
        public string ID { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }

        [Display(Name = "Student ID")]
        [Required]
        public string StudentID { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Assigned Class")]
        public Guid ClassID { get; set; }
        
    }
}