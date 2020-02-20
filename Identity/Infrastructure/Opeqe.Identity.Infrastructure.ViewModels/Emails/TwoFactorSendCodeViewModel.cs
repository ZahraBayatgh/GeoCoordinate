namespace Opeqe.Identity.Infrastructure.ViewModels.Emails
{
    public class TwoFactorSendCodeViewModel : EmailsBase
    {
        public string Token { set; get; }
    }
}