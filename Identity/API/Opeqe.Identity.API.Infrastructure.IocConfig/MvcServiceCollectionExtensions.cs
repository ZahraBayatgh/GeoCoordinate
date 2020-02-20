using DNTCommon.Web.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class MvcServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.UseYeKeModelBinder();
                options.AllowEmptyInputInBodyModelBinding = true;
                // options.Filters.Add(new NoBrowserCacheAttribute());
            })
             .AddNewtonsoftJson(jsonOptions =>
             {
                 jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                 jsonOptions.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                 jsonOptions.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
             }
           );
            return services;
        }
    }

}