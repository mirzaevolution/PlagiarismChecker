using System.ComponentModel.DataAnnotations;

namespace PlagiarismCore.Models.IdentityModels
{
    public class AssignmentModel
    {
        [Required]
        [Display(Name = "Subject Name")]
        public string AssignmentName { get; set; }
    }
}