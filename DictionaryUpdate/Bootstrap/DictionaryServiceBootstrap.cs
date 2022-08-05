using DictionaryService.Interfaces;
using DictionaryService.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DictionaryUpdate.Bootstrap
{
    public static class DictionaryServiceBootstrap
    {
        public static IServiceCollection AddDicionaryService(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<IDictService, DictService>();

            return services;
        }
    }
}
