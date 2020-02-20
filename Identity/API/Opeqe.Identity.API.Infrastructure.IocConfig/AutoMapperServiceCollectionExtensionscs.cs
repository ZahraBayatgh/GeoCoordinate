using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Opeqe.IdentityInfrastructure.Mapper;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class AutoMapperServiceCollectionExtensionscs
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }

}