// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DaprShowcase.Services.Orchestrator.Application.Dapr.Services;
using Microsoft.Azure.WebJobs;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr.Bindings
{
    class DaprPublishAsyncCollector : IAsyncCollector<DaprPubSubEvent>
    {
        readonly ConcurrentBag<DaprPubSubEvent> events = new ConcurrentBag<DaprPubSubEvent>();

        readonly DaprServiceClient daprClient;
        readonly DaprPublishAttribute attr;

        public DaprPublishAsyncCollector(DaprPublishAttribute attr, DaprServiceClient daprClient)
        {
            this.attr = attr;
            this.daprClient = daprClient;
        }

        public Task AddAsync(DaprPubSubEvent item, CancellationToken cancellationToken = default)
        {
            item.PubSubName ??= this.attr.PubSubName ?? throw new ArgumentException("No pub/sub name was found. Make sure it is configured either in the binding properties or in the data payload.", nameof(item));
            item.Topic ??= this.attr.Topic ?? throw new ArgumentException("No topic information was found. Make sure it is configured either in the binding properties or in the data payload.", nameof(item));

            this.events.Add(item);

            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default)
        {
            if (this.events.Count == 0)
            {
                return Task.CompletedTask;
            }

            // Publish all events in parallel
            //
            // Name and Topic cannot be null here - we verify them when the event is added.
            return Task.WhenAll(
                this.events.Select(
                    e => this.daprClient.PublishEventAsync(
                        this.attr.DaprAddress,
                        e.PubSubName!,
                        e.Topic!,
                        e.Payload,
                        cancellationToken)));
        }
    }
}
