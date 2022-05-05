// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DaprShowcase.Services.Orchestrator.Application.Dapr.Services;
using Microsoft.Azure.WebJobs;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr.Bindings
{
    class DaprBindingAsyncCollector : IAsyncCollector<DaprBindingMessage>
    {
        readonly ConcurrentQueue<DaprBindingMessage> requests = new ConcurrentQueue<DaprBindingMessage>();
        readonly DaprBindingAttribute attr;
        readonly DaprServiceClient daprService;

        public DaprBindingAsyncCollector(DaprBindingAttribute attr, DaprServiceClient daprService)
        {
            this.attr = attr;
            this.daprService = daprService;
        }

        public Task AddAsync(DaprBindingMessage item, CancellationToken cancellationToken = default)
        {
            item.BindingName ??= this.attr.BindingName ?? throw new ArgumentException("A non-null binding name must be specified");
            item.Operation ??= this.attr.Operation ?? throw new ArgumentException("A non-null operation must be specified");

            this.requests.Enqueue(item);
            return Task.CompletedTask;
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default)
        {
            while (this.requests.TryDequeue(out DaprBindingMessage? item))
            {
                await this.daprService.SendToDaprBindingAsync(
                    this.attr.DaprAddress,
                    item!,
                    cancellationToken);
            }
        }
    }
}