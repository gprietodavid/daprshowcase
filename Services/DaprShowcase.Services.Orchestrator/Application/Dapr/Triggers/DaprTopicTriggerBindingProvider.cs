// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CloudNative.CloudEvents.NewtonsoftJson;
using DaprShowcase.Services.Orchestrator.Application.Dapr.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Newtonsoft.Json.Linq;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr.Triggers
{
    internal class DaprTopicTriggerBindingProvider : ITriggerBindingProvider
    {
        private readonly DaprServiceListener serviceListener;
        private readonly INameResolver nameResolver;

        public DaprTopicTriggerBindingProvider(DaprServiceListener serviceListener, INameResolver nameResolver)
        {
            this.serviceListener = serviceListener ?? throw new ArgumentNullException(nameof(serviceListener));
            this.nameResolver = nameResolver;
        }

        public Task<ITriggerBinding?> TryCreateAsync(TriggerBindingProviderContext context)
        {
            ParameterInfo parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<DaprTopicTriggerAttribute>(inherit: false);
            if (attribute == null)
            {
                return Utils.NullTriggerBindingTask;
            }

            // Resolve names in pub/sub, topic, and route from settings
            if (!this.nameResolver.TryResolveWholeString(attribute.PubSubName, out var pubSubName))
            {
                pubSubName = attribute.PubSubName;
            }

            string topic = TriggerHelper.ResolveTriggerName(parameter, this.nameResolver, attribute.Topic);

            if (attribute.Route is null || !this.nameResolver.TryResolveWholeString(attribute.Route, out var route))
            {
                route = attribute.Route ?? topic;
            }

            if (!route.StartsWith("/"))
            {
                route = "/" + route;
            }

            return Task.FromResult<ITriggerBinding?>(
                new DaprTopicTriggerBinding(this.serviceListener, pubSubName, topic, route, parameter));
        }

        private class DaprTopicTriggerBinding : DaprTriggerBindingBase
        {
            static readonly JsonEventFormatter CloudEventFormatter = new JsonEventFormatter();

            private readonly DaprServiceListener serviceListener;
            private readonly string pubSubName;
            private readonly string topic;
            private readonly string route;

            public DaprTopicTriggerBinding(
                DaprServiceListener serviceListener,
                string pubSubName,
                string topic,
                string route,
                ParameterInfo parameter)
                : base(serviceListener, parameter)
            {
                this.serviceListener = serviceListener ?? throw new ArgumentNullException(nameof(serviceListener));
                this.pubSubName = pubSubName ?? throw new ArgumentNullException(nameof(pubSubName));
                this.topic = topic ?? throw new ArgumentNullException(nameof(topic));
                this.route = route ?? throw new ArgumentNullException(nameof(route));
            }

            protected override DaprListenerBase OnCreateListener(ITriggeredFunctionExecutor executor)
            {
                return new DaprTopicListener(this.serviceListener, executor, new DaprTopicSubscription(this.pubSubName, this.topic, this.route));
            }

            protected async override Task<object?> ConvertCloudEventFromJson(Stream inputStream, ValueBindingContext context)
            {
                // The input is always expected to be an object in the Cloud Events schema
                // https://github.com/cloudevents/spec/blob/v1.0/spec.md#example
                return await CloudEventFormatter.DecodeStructuredModeMessageAsync(inputStream, null, null);
            }

            protected override object? ConvertFromJson(JToken jsonValue, Type destinationType)
            {
                if (jsonValue is JObject jsonObject && jsonObject.TryGetValue("data", StringComparison.Ordinal, out JToken? eventData))
                    return base.ConvertFromJson(eventData, destinationType); // Do the generic conversion from the "data" payload

                return base.ConvertFromJson(jsonValue, destinationType);
            }

            private sealed class DaprTopicListener : DaprListenerBase
            {
                private readonly ITriggeredFunctionExecutor executor;
                private readonly DaprTopicSubscription topic;

                public DaprTopicListener(
                    DaprServiceListener serviceListener,
                    ITriggeredFunctionExecutor executor,
                    DaprTopicSubscription topic)
                    : base(serviceListener)
                {
                    this.executor = executor;
                    this.topic = topic;

                    serviceListener.RegisterTopic(this.topic);
                }

                public override void AddRoute(IRouteBuilder routeBuilder)
                {
                    // Example: POST /orders
                    // { "pubsubname": "pubsub", "topic": "newOrder", "route": "/orders"}
                    // https://docs.dapr.io/reference/api/pubsub_api/#provide-routes-for-dapr-to-deliver-topic-events
                    routeBuilder.MapPost(this.topic.Route, this.DispatchAsync);
                }

                public override async Task DispatchAsync(HttpContext context)
                {
                    var input = new TriggeredFunctionData
                    {
                        TriggerValue = context,
                    };

                    try
                    {
                        FunctionResult result = await this.executor.TryExecuteAsync(input, context.RequestAborted);
                    }
                    catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
                    {
                        // The request was aborted by the client. No-op.
                        // TODO: Consider moving exception handling into base class
                    }
                    catch (Exception)
                    {
                        // This means an unhandled exception occurred in the Functions runtime.
                        // This is often caused by the host shutting down while a function is still executing.
                        // TODO: Handle failure
                    }
                }
            }
        }
    }
}