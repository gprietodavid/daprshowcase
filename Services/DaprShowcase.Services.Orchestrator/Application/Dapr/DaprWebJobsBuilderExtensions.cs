// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System;
using DaprShowcase.Services.Orchestrator.Application.Dapr.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr
{
    /// <summary>
    /// Extension methods for Dapr integration.
    /// </summary>
    public static class DaprWebJobsBuilderExtensions
    {
        /// <summary>
        /// Adds the Dapr extension to the provided <see cref="IWebJobsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        /// <returns>Returns the updated webjobs builder.</returns>
        public static IWebJobsBuilder AddDapr(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<DaprExtensionConfigProvider>()
                .Services
                .AddSingleton<DaprServiceClient>()
                .AddSingleton<DaprServiceListener>()
                .AddHttpClient();

            return builder;
        }
    }
}
