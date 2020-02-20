using HSP.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace HSP.Infrastructure.IocConfig
{
    public static class RedisServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomRedis(this IServiceCollection services, SiteSettings siteSettings)
        {
            var redisConfiguration = siteSettings.Redis;// _config.GetSection("Redis").Get<RedisConfiguration>();

            services.AddSingleton(redisConfiguration);
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<IRedisDefaultCacheClient, RedisDefaultCacheClient>();
            services.AddSingleton<ISerializer, NewtonsoftSerializer>();

            return services;
        }
    }

}