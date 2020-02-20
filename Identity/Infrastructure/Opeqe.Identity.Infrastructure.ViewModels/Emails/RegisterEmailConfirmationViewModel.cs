using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.ViewModels.Emails
{
    public class RegisterEmailConfirmationViewModel : EmailsBase
    {
        public User User { set; get; }
        public string EmailConfirmationToken { set; get; }
    }
}
