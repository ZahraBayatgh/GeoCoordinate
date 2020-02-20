using System.ComponentModel.DataAnnotations;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class ForgotPasswordViewModel2
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

}