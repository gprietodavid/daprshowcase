// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using DaprShowcase.Services.Orchestrator.Application.Dapr.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Host.Listeners;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr.Triggers
{
    abstract class DaprListenerBase : IListener
    {
        readonly DaprServiceListener serviceListener;
        private bool disposedValue;

        protected DaprListenerBase(DaprServiceListener serviceListener)
        {
            this.serviceListener = serviceListener;
        }

        public abstract void AddRoute(IRouteBuilder routeBuilder);

        public virtual void Cancel()
        {
            // no-op by default
        }

        Task IListener.StartAsync(CancellationToken cancellationToken)
        {
            return this.serviceListener.EnsureStartedAsync(cancellationToken);
        }

        Task IListener.StopAsync(CancellationToken cancellationToken)
        {
            return this.serviceListener.DeregisterListenerAsync(this, cancellationToken);
        }

        public abstract Task DispatchAsync(HttpContext context);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
