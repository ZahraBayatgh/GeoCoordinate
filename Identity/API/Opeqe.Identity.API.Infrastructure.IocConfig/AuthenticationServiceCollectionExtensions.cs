using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Opeqe.Identity.Infrastructure.Settings;
using System.Text;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IdentitySettings siteSettings)
        {
            services.AddAuthentication(
                options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = siteSettings.Tokens.Issuer,
                        ValidAudience = siteSettings.Tokens.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siteSettings.Tokens.Key))
                    };
                });

            return services;
        }
    }
}