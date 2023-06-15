using Microsoft.Extensions.Options;
using Simpra.Api.Settings;
using Simpra.Service.Service.Concrete;

namespace Simpra.Api.Extensions
{
    public static class RedisExtension
    {
        public static void AddRedisExtension(this IServiceCollection services, IConfiguration configuration)
        {
            //Options Pattern
            services.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));

            services.AddSingleton<RedisService>(sp =>
            {
                var redisSettings = sp.GetService<IOptions<RedisSettings>>().Value;
                var redis = new RedisService(redisSettings.Host, redisSettings.Port);
                redis.Connect();
                return redis;
            });
        }



    }
}
