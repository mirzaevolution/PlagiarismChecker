using System.ComponentModel.DataAnnotations;

namespace PlagiarismCore.Models.IdentityModels
{
    public class AssignmentModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Subject Name")]
        public string AssignmentName { get; set; }
    }
}