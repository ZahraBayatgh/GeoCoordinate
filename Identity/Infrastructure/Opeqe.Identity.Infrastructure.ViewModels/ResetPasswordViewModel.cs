using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} and at most {1} letters.", MinimumLength = 6)]
        [Remote("ValidatePassword", "ForgotPassword",
            AdditionalFields = nameof(Email) + "," + ViewModelConstants.AntiForgeryToken, HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare(nameof(Password), ErrorMessage = "The entered passwords do not match")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
    public class ResetPasswordViewModel2
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} and at most {1} letters.", MinimumLength = 6)]
        [Remote("ValidatePassword", "ForgotPassword",
            AdditionalFields = nameof(UserName) + "," + ViewModelConstants.AntiForgeryToken, HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare(nameof(Password), ErrorMessage = "The entered passwords do not match")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

}