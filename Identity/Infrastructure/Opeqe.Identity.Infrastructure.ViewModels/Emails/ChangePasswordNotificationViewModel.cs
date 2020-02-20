using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.ViewModels.Emails
{
    public class ChangePasswordNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}