using System;
using System.Collections.Generic;
using System.Text;
using DaprShowcase.Services.Orchestrator.Application.Dapr.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr
{
    public static class DaprFunctionsHostBuilderExtensions
    {
        public static IFunctionsHostBuilder AddDapr(this IFunctionsHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var webJobsBuilder = builder.Services.AddWebJobs(x => { return; });

            webJobsBuilder.AddExtension<DaprExtensionConfigProvider>()
                .Services
                .AddSingleton<DaprServiceClient>()
                .AddSingleton<DaprServiceListener>()
                .AddHttpClient();

            return builder;
        }
    }
}