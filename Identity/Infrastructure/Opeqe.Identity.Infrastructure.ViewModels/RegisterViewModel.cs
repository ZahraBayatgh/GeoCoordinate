using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}