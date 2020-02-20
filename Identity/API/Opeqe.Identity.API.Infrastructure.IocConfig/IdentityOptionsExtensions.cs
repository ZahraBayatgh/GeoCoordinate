using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Services;
using Opeqe.Identity.Infrastructure.Settings;
using System;
using System.Threading.Tasks;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class IdentityOptionsServiceCollectionExtensions
    {
        public const string EmailConfirmationTokenProviderName = "ConfirmEmail";

        public static IServiceCollection AddIdentityOptions(
            this IServiceCollection services, IdentitySettings siteSettings)
        {
            if (siteSettings == null)
            {
                throw new ArgumentNullException(nameof(siteSettings));
            }

            services.addConfirmEmailDataProtectorTokenOptions(siteSettings);
            services.AddIdentity<User, Role>(identityOptions =>
            {
                setPasswordOptions(identityOptions.Password, siteSettings);
                setSignInOptions(identityOptions.SignIn, siteSettings);
                setUserOptions(identityOptions.User);
                setLockoutOptions(identityOptions.Lockout, siteSettings);
            })
            .AddUserStore<ApplicationUserStore>()
              .AddUserManager<ApplicationUserManager>()
              .AddRoleStore<ApplicationRoleStore>()
              .AddRoleManager<ApplicationRoleManager>()
              .AddSignInManager<ApplicationSignInManager>()
              .AddErrorDescriber<CustomIdentityErrorDescriber>()
              // You **cannot** use .AddEntityFrameworkStores() when you customize everything
              //.AddEntityFrameworkStores<MarketDbContext, int>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<ConfirmEmailDataProtectorTokenProvider<User>>(EmailConfirmationTokenProviderName);



            services.enableImmediateLogout();

            return services;
        }

        private static void addConfirmEmailDataProtectorTokenOptions(this IServiceCollection services, IdentitySettings siteSettings)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider = EmailConfirmationTokenProviderName;
            });

            services.Configure<ConfirmEmailDataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = siteSettings.EmailConfirmationTokenProviderLifespan;
            });
        }

        private static void enableImmediateLogout(this IServiceCollection services)
        {
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // enables immediate logout, after updating the user's stat.
                options.ValidationInterval = TimeSpan.Zero;
                options.OnRefreshingPrincipal = principalContext =>
                {
                    // Invoked when the default security stamp validator replaces the user's ClaimsPrincipal in the cookie.

                    //var newId = new ClaimsIdentity();
                    //newId.AddClaim(new Claim("PreviousName", principalContext.CurrentPrincipal.Identity.Name));
                    //principalContext.NewPrincipal.AddIdentity(newId);

                    return Task.CompletedTask;
                };
            });
        }

        private static void setLockoutOptions(LockoutOptions identityOptionsLockout, IdentitySettings siteSettings)
        {
            identityOptionsLockout.AllowedForNewUsers = siteSettings.LockoutOptions.AllowedForNewUsers;
            identityOptionsLockout.DefaultLockoutTimeSpan = siteSettings.LockoutOptions.DefaultLockoutTimeSpan;
            identityOptionsLockout.MaxFailedAccessAttempts = siteSettings.LockoutOptions.MaxFailedAccessAttempts;
        }

        private static void setPasswordOptions(PasswordOptions identityOptionsPassword, IdentitySettings siteSettings)
        {
            identityOptionsPassword.RequireDigit = siteSettings.PasswordOptions.RequireDigit;
            identityOptionsPassword.RequireLowercase = siteSettings.PasswordOptions.RequireLowercase;
            identityOptionsPassword.RequireNonAlphanumeric = siteSettings.PasswordOptions.RequireNonAlphanumeric;
            identityOptionsPassword.RequireUppercase = siteSettings.PasswordOptions.RequireUppercase;
            identityOptionsPassword.RequiredLength = siteSettings.PasswordOptions.RequiredLength;
        }

        private static void setSignInOptions(SignInOptions identityOptionsSignIn, IdentitySettings siteSettings)
        {
            identityOptionsSignIn.RequireConfirmedEmail = siteSettings.EnableEmailConfirmation;
        }

        private static void setUserOptions(UserOptions identityOptionsUser)
        {
            identityOptionsUser.RequireUniqueEmail = false;
        }
    }
}