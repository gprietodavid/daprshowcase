// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using DaprShowcase.Services.Orchestrator.Application.Dapr;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(DaprWebJobsStartup))]

namespace DaprShowcase.Services.Orchestrator.Application.Dapr
{
    /// <summary>
    /// Startup configuration class for the Dapr extension.
    /// </summary>
    public class DaprWebJobsStartup : IWebJobsStartup
    {
        /// <summary>
        /// Adds the Dapr extension to the WebJobs builder.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDapr();
        }
    }
}
