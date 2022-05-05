using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DaprShowcase.Services.ZipWorker
{
    class Program
    {
        public const int DEFAULT_HTTP2_PORT = 5050;

        private static IHostBuilder CreateHostBuilder(string[] args, string hostUrl = null, bool useAzureAppConfiguration = true, Action<IConfigurationBuilder> configureAppConfiguration = null)
        {
            IConfigurationRoot configuration = null;

            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostBuilder, configBuilder) =>
                {
                    var env = hostBuilder.HostingEnvironment;

                    configBuilder
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<Startup>(true, true);

                    configureAppConfiguration?.Invoke(configBuilder);
                })
                .ConfigureAppConfiguration((hostBuilder, configBuilder) =>
                {
                    configuration = configBuilder.Build();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => { options.ListenLocalhost(configuration.GetValue<int?>("Host:Http2Port") ?? DEFAULT_HTTP2_PORT, o => o.Protocols = HttpProtocols.Http2); });
                    webBuilder.UseStartup<Startup>();
                });
        }

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args);
            await host.Build().RunAsync();
        }
    }
}
