using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class ConfirmEmailDataProtectionTokenProviderOptions : DataProtectionTokenProviderOptions
    { }

    public class ConfirmEmailDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public ConfirmEmailDataProtectorTokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<ConfirmEmailDataProtectionTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
            // NOTE: DataProtectionTokenProviderOptions.TokenLifespan is set to TimeSpan.FromDays(1)
            // which is low for the `ConfirmEmail` task.
        }
    }
}