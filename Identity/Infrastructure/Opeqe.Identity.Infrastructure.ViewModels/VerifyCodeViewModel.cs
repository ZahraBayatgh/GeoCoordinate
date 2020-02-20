using System.ComponentModel.DataAnnotations;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Display(Name = "Security code")]
        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember the current browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remembering Validation?")]
        public bool RememberMe { get; set; }
    }
}