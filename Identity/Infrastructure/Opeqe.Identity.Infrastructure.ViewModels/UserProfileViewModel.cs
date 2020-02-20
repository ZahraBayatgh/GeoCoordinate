using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class UserProfileViewModel
    {
        public const string AllowedImages = ".png,.jpg,.jpeg,.gif";

        [Required]
        [Display(Name = "User name")]
        [Remote("ValidateUsername", "UserProfile",
            AdditionalFields = nameof(Email) + "," + ViewModelConstants.AntiForgeryToken + "," + nameof(Pid),
            HttpMethod = "POST")]
        public string UserName { get; set; }

        [Display(Name = "Name")]
        [Required]
        [StringLength(450)]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required]
        [StringLength(450)]
        public string LastName { get; set; }

        [Required]
        [Remote("ValidateUsername", "UserProfile",
            AdditionalFields = nameof(UserName) + "," + ViewModelConstants.AntiForgeryToken + "," + nameof(Pid),
            HttpMethod = "POST")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "تصویر")]
        [StringLength(maximumLength: 1000, ErrorMessage = "The maximum image address length is 1000 characters.")]
        public string PhotoFileName { set; get; }

        [UploadFileExtensions(AllowedImages,
            ErrorMessage = "Please only take one picture " + AllowedImages + " Submit.")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }

        public int? DateOfBirthYear { set; get; }
        public int? DateOfBirthMonth { set; get; }
        public int? DateOfBirthDay { set; get; }

        [Display(Name = "Residence")]
        public string Location { set; get; }

        [Display(Name = "Public Email Display")]
        public bool IsEmailPublic { set; get; }

        [Display(Name = "Enable two-step validation")]
        public bool TwoFactorEnabled { set; get; }

        public bool IsPasswordTooOld { set; get; }

        [HiddenInput]
        public string Pid { set; get; }

        [HiddenInput]
        public bool IsAdminEdit { set; get; }
    }
}