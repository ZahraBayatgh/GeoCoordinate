using HSP.Infrastructure.Data.Context;
using HSP.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace HSP.NGT.API.Infrastructure.IocConfig
{
    public static class MongoContextServiceCollectionExtentions
    {
        public static IServiceCollection AddMongoContext(this IServiceCollection services, SiteSettings settings)
        {
            services.AddScoped(sp => new MongoContext(settings.ConnectionStrings.MongoDb.ConnectionString, settings.ConnectionStrings.MongoDb.Database));
            return services;
        }

    }
}
