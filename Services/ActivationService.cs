using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.JSInterop;
using Oop.Client.Interfaces;

namespace Oop.Client.Services
{
    public static class ActivationServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            //Run the Blazor services
            AddServices(services);

            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddAuthorizationCore();
            services.AddSingleton(serviceProvider => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());
#pragma warning disable CA1416 // Validate platform compatibility
            services.TryAddSingleton<IIndexDBService, IndexDBService>();
            services.TryAddSingleton<FakeService>();
#pragma warning restore CA1416 // Validate platform compatibility
        } 

        public static async Task RunServicesAsync(this IServiceProvider services)
        {
           await (services.GetService<FakeService>() as IInitialize).Initialize();
        }
    }
}
