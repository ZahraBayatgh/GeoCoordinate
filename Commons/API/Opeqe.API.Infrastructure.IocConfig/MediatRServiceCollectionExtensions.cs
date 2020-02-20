using HSP.Application.Behaviors;
using HSP.Infrastructure.Data.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HSP.NGT.API.Infrastructure.IocConfig
{
    public static class MediatRServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().
           Where(assembly => assembly.GetName().Name.EndsWith("Commands")
           || assembly.GetName().Name.EndsWith("Queries")).ToArray();
            services.AddMediatR(assemblies);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));

            return services;
        }
    }
    public static class MongoDbRepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDBRepository(this IServiceCollection services)
        {

            services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));

            return services;
        }
    }

}
