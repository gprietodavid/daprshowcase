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
using DaprShowcase.Common.Adapters;
using DaprShowcase.Common.Adapters.AuditingPublisher;
using DaprShowcase.Services.EntitiesApi.Adapters;
using DaprShowcase.Services.EntitiesApi.Adapters.CompanyStorage;
using DaprShowcase.Services.EntitiesApi.Adapters.SequenceStorage;

namespace DaprShowcase.Services.EntitiesApi
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
            services.AddTransient<ICompanyStorageAdapter, CompanyStorageAdapter>();
            services.AddTransient<ISequenceStorageAdapter, SequenceStorageAdapter>();
            services.AddTransient<IAuditingPublisherAdapter, AuditingPublisherAdapter>();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dapr Showcase Entities API", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dapr Showcase Entities API v1"));
            }
            
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
