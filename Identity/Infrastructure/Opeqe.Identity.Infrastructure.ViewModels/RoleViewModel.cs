using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class RoleViewModel
    {
        [HiddenInput]
        public string Id { set; get; }

        [Required]
        [Display(Name = "Role name")]
        public string Name { set; get; }
    }
}