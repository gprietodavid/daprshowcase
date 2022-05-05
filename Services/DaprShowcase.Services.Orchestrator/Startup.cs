using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Dapr;
using DaprShowcase.Services.Orchestrator.Adapters;
using DaprShowcase.Services.Orchestrator.Application.Dapr;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(DaprShowcase.Services.Orchestrator.Startup))]

namespace DaprShowcase.Services.Orchestrator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddDapr();
            builder.Services.AddDaprClient();
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IWorkflowPublisherAdapter, WorkflowPublisherAdapter>();
        }
    }
}