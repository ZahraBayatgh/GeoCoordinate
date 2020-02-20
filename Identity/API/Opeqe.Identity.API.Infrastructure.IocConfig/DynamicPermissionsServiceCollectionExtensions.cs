using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Opeqe.Identity.Infrastructure.Services;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class DynamicPermissionsServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicPermissions(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, DynamicPermissionsAuthorizationHandler>();
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(
                    name: ConstantPolicies.DynamicPermission,
                    configurePolicy: policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.Requirements.Add(new DynamicPermissionRequirement());
                    });
            });

            return services;
        }
    }
}