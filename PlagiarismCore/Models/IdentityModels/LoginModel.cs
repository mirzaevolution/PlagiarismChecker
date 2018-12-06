using System.ComponentModel.DataAnnotations;

namespace PlagiarismCore.Models.IdentityModels
{
    public class LoginModel
    {
        [Display(Name = "ID")]
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me ?")]
        public string RememberMe { get; set; }
    }
}