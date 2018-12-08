using System;
using System.ComponentModel.DataAnnotations;

namespace PlagiarismCore.Models.IdentityModels
{
    public class StudentClass
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required,Display(Name = "Class Name")]
        public string ClassName { get; set; }
    }
}