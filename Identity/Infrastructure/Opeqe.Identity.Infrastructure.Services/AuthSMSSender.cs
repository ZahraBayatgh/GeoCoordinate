using DNTCommon.Web.Core;
using Microsoft.Extensions.Configuration;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using SmsIrRestful;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class AuthSMSSender : ISmsSender
    {
        private readonly IConfiguration _settings;
        private readonly SmtpConfig smtp;

        public AuthSMSSender(IConfiguration settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

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

        public Task SendSmsAsync(string number, string message)
        {
            if (smtp.UsePickupFolder)
            {
                string path = $@"{smtp.PickupFolder}\{number}.txt";
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine($"{number}");
                        sw.WriteLine($"Verification code: {message}");
                    }
                }
            }
            else
            {
                SmsIrRestful.Token tk = new SmsIrRestful.Token();
                string result = tk.GetToken("f102ae793839867ae1ee90f4", "d@k@nB2z1&2&3@VN@87134");

                RestVerificationCode messageSendObject = new RestVerificationCode()
                {
                    Code = message,
                    MobileNumber = number
                };
                RestVerificationCodeRespone restVerificationCodeRespone = new VerificationCode().Send(result, messageSendObject);
            }

            return Task.FromResult(0);
        }
    }
}