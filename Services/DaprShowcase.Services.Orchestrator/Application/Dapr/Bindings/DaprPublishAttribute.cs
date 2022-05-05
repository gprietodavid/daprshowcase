// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System;
using Microsoft.Azure.WebJobs.Description;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr.Bindings
{
    /// <summary>
    /// Attribute to specify parameters for the Dapr-publish output binding.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public class DaprPublishAttribute : DaprBaseAttribute
    {
        /// <summary>
        /// Gets or sets the pub/sub name to publish to.
        /// </summary>
        [AutoResolve]
        public string? PubSubName { get; set; }

        /// <summary>
        /// Gets or sets the name of the topic to publish to.
        /// </summary>
        [AutoResolve]
        public string? Topic { get; set; }
    }
}
