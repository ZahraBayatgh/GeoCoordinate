using AutoMapper;
using HSP.NGT.API.Infrastructure.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace HSP.NGT.API.Infrastructure.IocConfig
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
