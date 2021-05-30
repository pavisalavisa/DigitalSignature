using Common.DateTime;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public static class DependencyModule
    {
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
