using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Services;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Identity.Infrastructure.Services.Logger;
using System.Security.Claims;
using System.Security.Principal;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class ServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPrincipal>(provider =>
                provider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current);

            services.AddScoped<ILookupNormalizer, CustomNormalizer>();

            services.AddScoped<ISecurityStampValidator, CustomSecurityStampValidator>();
            services.AddScoped<SecurityStampValidator<User>, CustomSecurityStampValidator>();

            services.AddScoped<IPasswordValidator<User>, CustomPasswordValidator>();
            services.AddScoped<PasswordValidator<User>, CustomPasswordValidator>();

            services.AddScoped<IUserValidator<User>, CustomUserValidator>();
            services.AddScoped<UserValidator<User>, CustomUserValidator>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsPrincipalFactory>();
            services.AddScoped<UserClaimsPrincipalFactory<User, Role>, ApplicationClaimsPrincipalFactory>();

            services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

            services.AddScoped<IApplicationUserStore, ApplicationUserStore>();
            services.AddScoped<UserStore<User, Role, CustomIdentityDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>, ApplicationUserStore>();

            services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();

            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();

            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();

            services.AddScoped<IApplicationRoleStore, ApplicationRoleStore>();
            services.AddScoped<RoleStore<Role, CustomIdentityDbContext, int, UserRole, RoleClaim>, ApplicationRoleStore>();

            services.AddScoped<IEmailSender, AuthMessageSender>();

            // services.AddSingleton<IAntiforgery, NoBrowserCacheAntiforgery>();
            // services.AddSingleton<IHtmlGenerator, NoBrowserCacheHtmlGenerator>();

            services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
            services.AddScoped<IUsedPasswordsService, UsedPasswordsService>();
            services.AddScoped<ISiteStatService, SiteStatService>();
            services.AddScoped<IUsersPhotoService, UsersPhotoService>();
            services.AddScoped<ISecurityTrimmingService, SecurityTrimmingService>();
            services.AddScoped<IAppLogItemsService, AppLogItemsService>();

            services.AddScoped<IJobsService, JobsService>();

            return services;
        }
    }

}