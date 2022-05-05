using System;
using DaprShowcase.Common;
using DaprShowcase.Common.Adapters.OrchestratorPublisher;
using DaprShowcase.Services.ZipWorker.Adapters;
using DaprShowcase.Services.ZipWorker.Application.Handlers.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaprShowcase.Services.ZipWorker
{
    public class Startup
    {
        private const string GRPC_ERROR = "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Console.WriteLine((configuration as IConfigurationRoot)?.GetDebugView());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<WorkerProcessService>();
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync(GRPC_ERROR); });
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetryWorkerService("ccebd792-5e1b-4fb9-b0a3-fdb72ada53ab");
            services.AddGrpc();
            services.AddDaprClient();
            services.AddSingleton<IServiceProvider>(sp => sp);
            services.AddTransient<IDocumentBlobStorageAdapter, DocumentBlobStorageAdapter>();
            services.AddTransient<IOrchestratorPublisherAdapter, OrchestratorPublisherAdapter>();
            services.AddTransient<ZipBlobCommandHandler>();
            services.AddTransient<IMessageSubscriptionCollection, Subscriptions>();
        }
    }
}