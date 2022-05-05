using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Services.AuditingWorker.Adapters;
using DaprShowcase.Services.AuditingWorker.Application.Handlers.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DaprShowcase.Services.AuditingWorker
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
            services.AddTransient<IAuditRecordStorageAdapter, AuditRecordStorageAdapter>();
            services.AddSingleton<IServiceProvider>(sp => sp);
            services.AddSingleton<ISequenceAdapter, SequenceAdapter>(sp => new SequenceAdapter());
            services.AddTransient<OnDomainUpdatedEventHandler>();
            services.AddTransient<OnSequenceItemCreatedEventHandler>();
        }
    }
}