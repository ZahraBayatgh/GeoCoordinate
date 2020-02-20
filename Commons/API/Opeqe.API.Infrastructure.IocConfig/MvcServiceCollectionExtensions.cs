using DNTCommon.Web.Core;
using Microsoft.Extensions.DependencyInjection;

namespace HSP.Infrastructure.IocConfig
{
    public static class MvcServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.UseYeKeModelBinder();
                options.AllowEmptyInputInBodyModelBinding = true;
                // options.Filters.Add(new NoBrowserCacheAttribute());
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
            });

            return services;
        }
    }

}