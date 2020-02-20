using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Identity;
using System;

namespace Opeqe.Identity.Infrastructure.Settings
{
    public class IdentitySettings
    {
        public ActiveDatabase ActiveDatabase { get; set; }

        public AdminUserSeed AdminUserSeed { get; set; }
        public Connectionstrings ConnectionStrings { get; set; }

        public Logging Logging { get; set; }
        public UserAvatarImageOptions UserAvatarImageOptions { get; set; }

        public SmtpConfig Smtp { get; set; }
        public bool EnableEmailConfirmation { get; set; }
        public TimeSpan EmailConfirmationTokenProviderLifespan { get; set; }
        public int NotAllowedPreviouslyUsedPasswords { get; set; }
        public int ChangePasswordReminderDays { get; set; }
        public PasswordOptions PasswordOptions { get; set; }
        public string UsersAvatarsFolder { get; set; }
        public string UserDefaultPhoto { get; set; }
        public LockoutOptions LockoutOptions { get; set; }
        public string[] EmailsBanList { get; set; }
        public string[] PasswordsBanList { get; set; }
        public Token Tokens { get; set; }
        public string IdentityConsumerAPIAddress { get; set; }
        public int EventBusRetryCount { get; set; }
        public string EventBusConnection { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
        public string SubscriptionClientName { get; set; }
    }
}