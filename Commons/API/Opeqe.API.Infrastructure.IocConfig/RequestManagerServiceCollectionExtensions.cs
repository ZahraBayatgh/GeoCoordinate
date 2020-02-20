using HSP.Infrastructure.Idempotency;
using Microsoft.Extensions.DependencyInjection;

namespace HSP.NGT.API.Infrastructure.IocConfig
{
    public static class RequestManagerServiceCollectionExtensionscs
    {
        public static IServiceCollection AddRequestManager(this IServiceCollection services)
        {
            services.AddScoped<IRequestManager, RequestManager>();

            return services;
        }
    }
}
