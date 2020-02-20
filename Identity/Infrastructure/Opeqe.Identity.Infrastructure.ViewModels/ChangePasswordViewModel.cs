using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} and at most {1} letters.", MinimumLength = 6)]
        [Remote("ValidatePassword", "ChangePassword",
            AdditionalFields = ViewModelConstants.AntiForgeryToken, HttpMethod = "POST")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat new password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The entered passwords do not match")]
        public string ConfirmPassword { get; set; }

        public DateTime? LastUserPasswordChangeDate { get; set; }
    }
}