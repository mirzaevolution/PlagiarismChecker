using System.ComponentModel.DataAnnotations;

namespace PlagiarismCore.Models.IdentityModels
{
    public class AdminProfile
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [MinLength(6)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}