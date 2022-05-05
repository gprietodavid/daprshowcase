using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaprShowcase.Common.Adapters.BlobStorage;
using DaprShowcase.Common.Adapters.OrchestratorPublisher;
using DaprShowcase.Common.Adapters.WorkflowPublisher;
using DaprShowcase.Services.DocumentsApi.Adapters;
using DaprShowcase.Services.DocumentsApi.Adapters.CompanyFolderStorage;
using DaprShowcase.Services.DocumentsApi.Adapters.DocumentBlobStorage;
using DaprShowcase.Services.DocumentsApi.Adapters.EntitiesApi;
using DaprShowcase.Services.DocumentsApi.Adapters.WorkflowPublisher;

namespace DaprShowcase.Services.DocumentsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddDapr();
            services.AddDaprClient();
            services.AddTransient<IEntitiesApiAdapter, EntitiesApiAdapter>();
            services.AddTransient<ICompanyFolderStorageAdapter, CompanyFolderStorageAdapter>();
            services.AddTransient<IDocumentBlobStorageAdapter, DocumentBlobStorageAdapter>();
            services.AddTransient<IWorkflowPublisherAdapter, WorkflowPublisherAdapter>();
            services.AddTransient<IOrchestratorPublisherAdapter, OrchestratorPublisherAdapter>();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dapr Showcase Documents API", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dapr Showcase Documents API v1"));
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
