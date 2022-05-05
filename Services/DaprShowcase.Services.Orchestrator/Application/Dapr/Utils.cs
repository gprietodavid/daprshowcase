// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr
{
    static class Utils
    {
        public static readonly Task<ITriggerBinding?> NullTriggerBindingTask =
            Task.FromResult<ITriggerBinding?>(null);
    }
}
