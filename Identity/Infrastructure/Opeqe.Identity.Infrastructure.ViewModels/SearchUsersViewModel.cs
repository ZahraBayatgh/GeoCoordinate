using System.ComponentModel.DataAnnotations;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class SearchUsersViewModel
    {
        [Display(Name = "Phrase Search")]
        public string TextToFind { set; get; }

        [Display(Name = "Part of the email")]
        public bool IsPartOfEmail { set; get; }

        [Display(Name = "User number")]
        public bool IsUserId { set; get; }

        [Display(Name = "Part of the name")]
        public bool IsPartOfName { set; get; }

        [Display(Name = "Part of last name")]
        public bool IsPartOfLastName { set; get; }

        [Display(Name = "Part of the username")]
        public bool IsPartOfUserName { set; get; }

        [Display(Name = "Part of the accommodation")]
        public bool IsPartOfLocation { set; get; }

        [Display(Name = "Has verified email")]
        public bool HasEmailConfirmed { set; get; }

        [Display(Name = "Active only")]
        public bool UserIsActive { set; get; }

        [Display(Name = "Active and inactive users")]
        public bool ShowAllUsers { set; get; }

        [Display(Name = "Has a locked account")]
        public bool UserIsLockedOut { set; get; }

        [Display(Name = "Two-step validation")]
        public bool HasTwoFactorEnabled { set; get; }

        [Display(Name = "Return row number")]
        [Required]
        [Range(1, 1000, ErrorMessage = "The number entered must be in the range 1 to 1000")]
        public int MaxNumberOfRows { set; get; }

        public PagedUsersListViewModel PagedUsersList { set; get; }

        public SearchUsersViewModel()
        {
            ShowAllUsers = true;
            MaxNumberOfRows = 7;
        }
    }
}