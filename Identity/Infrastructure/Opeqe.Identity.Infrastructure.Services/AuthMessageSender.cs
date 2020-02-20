using DNTCommon.Web.Core;
using Microsoft.Extensions.Configuration;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Infrastructure.Toolkits.GuardToolkit;
using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class AuthMessageSender : IEmailSender
    {
        //private readonly IdentitySettings _settings;
        private readonly IWebMailService _webMailService;
        private readonly IConfiguration _settings;
        private readonly SmtpConfig smtp;

        public AuthMessageSender(
            IConfiguration smtpConfig,
            IWebMailService webMailService)
        {
            _settings = smtpConfig;
            _settings.CheckArgumentIsNull(nameof(_settings));

            _webMailService = webMailService;
            _webMailService.CheckArgumentIsNull(nameof(webMailService));

            smtp = new SmtpConfig
            {
                FromAddress = _settings["smtp:FromAddress"],
                LocalDomain = _settings["smtp:LocalDomain"],
                FromName = _settings["smtp:FromName"],
                Password = _settings["smtp:Password"],
                PickupFolder = _settings["smtp:PickupFolder"],
                UsePickupFolder = _settings.GetValue<bool>("smtp:UsePickupFolder"),
                Port = _settings.GetValue<int>("smtp:Port"),
                Server = _settings["smtp:Server"],
                Username = _settings["smtp:Username"],
            };
        }

        public Task SendEmailAsync<T>(string email, string subject, string viewNameOrPath, T model)
        {


            return _webMailService.SendEmailAsync(
                smtp,
                new[] { new MailAddress { ToName = "", ToAddress = email } },
                subject,
                viewNameOrPath,
                model
            );
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return _webMailService.SendEmailAsync(
                smtp,
                new[] { new MailAddress { ToName = "", ToAddress = email } },
                subject,
                message
            );
        }

    }
}