using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.ViewModels.Emails
{
    public class UserProfileUpdateNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}