// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;
using DaprShowcase.Services.Orchestrator.Application.Dapr.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr.Triggers
{
    class DaprServiceInvocationTriggerBindingProvider : ITriggerBindingProvider
    {
        readonly DaprServiceListener serviceListener;
        readonly INameResolver nameResolver;

        public DaprServiceInvocationTriggerBindingProvider(DaprServiceListener serviceListener, INameResolver resolver)
        {
            this.serviceListener = serviceListener ?? throw new ArgumentNullException(nameof(serviceListener));
            this.nameResolver = resolver;
        }

        public Task<ITriggerBinding?> TryCreateAsync(TriggerBindingProviderContext context)
        {
            ParameterInfo parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<DaprServiceInvocationTriggerAttribute>(inherit: false);
            if (attribute == null)
            {
                return Utils.NullTriggerBindingTask;
            }

            string methodName = TriggerHelper.ResolveTriggerName(parameter, this.nameResolver, attribute.MethodName);

            return Task.FromResult<ITriggerBinding?>(
                new DaprServiceInvocationTriggerBinding(this.serviceListener, methodName, parameter));
        }

        class DaprServiceInvocationTriggerBinding : DaprTriggerBindingBase
        {
            readonly DaprServiceListener serviceListener;
            readonly string methodName;

            public DaprServiceInvocationTriggerBinding(
                DaprServiceListener serviceListener,
                string methodName,
                ParameterInfo parameter)
                : base(serviceListener, parameter)
            {
                this.serviceListener = serviceListener ?? throw new ArgumentNullException(nameof(serviceListener));
                this.methodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            }

            protected override DaprListenerBase OnCreateListener(ITriggeredFunctionExecutor executor)
            {
                return new DaprServiceInvocationListener(this.serviceListener, executor, this.methodName);
            }

            sealed class DaprServiceInvocationListener : DaprListenerBase
            {
                readonly ITriggeredFunctionExecutor executor;
                readonly string methodName;

                public DaprServiceInvocationListener(
                    DaprServiceListener serviceListener,
                    ITriggeredFunctionExecutor executor,
                    string methodName)
                    : base(serviceListener)
                {
                    this.executor = executor;
                    this.methodName = methodName;
                }

                public override void AddRoute(IRouteBuilder routeBuilder)
                {
                    routeBuilder.MapPost(this.methodName, this.DispatchAsync);
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
