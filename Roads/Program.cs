using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Roads.Services;
using Roads.Services.Conracts;

namespace Roads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services
                .AddScoped<IDataProvider, FileDataProvider>()
                .AddSingleton<IRoadsService, RoadsService>()
                .AddLogging(cfg => cfg.AddConsole());

            using IHost host = builder.Build();

            Run(host.Services);
        }

        static void Run(IServiceProvider hostProvider)
        {
            using var serviceScope = hostProvider.CreateScope();
            var provider = serviceScope.ServiceProvider;
            provider.GetRequiredService<IRoadsService>().ProcessRoads();
        }
    }
}
/*
 * TODO:
 *      
 * 
 */
