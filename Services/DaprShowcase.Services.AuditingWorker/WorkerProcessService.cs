using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Handlers.Events;
using DaprShowcase.Common.Application.Messages.Events;
using DaprShowcase.Services.AuditingWorker.Application;
using DaprShowcase.Services.AuditingWorker.Application.Handlers.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;

namespace DaprShowcase.Services.AuditingWorker
{
    public sealed class WorkerProcessService : AppCallback.AppCallbackBase
    {
        private const string AUDITING_PUBSUB_NAME = "auditingpubsub";
        private const string AUDITING_PUBSUB_TOPIC = "daprshowcase";

        private readonly ILogger<WorkerProcessService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public WorkerProcessService(ILogger<WorkerProcessService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private TopicEventResponse HandleEventData<TEventData, TEventHandler>(string content)
            where TEventData : IEventData
            where TEventHandler : class, IEventHandler<TEventData>
        {
            var data = JsonConvert.DeserializeObject<string>(content);
            var @event = JsonConvert.DeserializeObject<TEventData>(data);
            var handler = _serviceProvider.GetService(typeof(TEventHandler)) as TEventHandler;
            var retrySpanFunc = new Func<int, TimeSpan>((int i) => new TimeSpan(0, 0, 0, 20));
            var onHandlerExceptionAction = new Action<Exception>((ex) => { _logger.LogCritical(ex, ex.Message); });

            handler?.HandleWithRetryAsync(@event, 3, retrySpanFunc, onHandlerExceptionAction);
            
            return new TopicEventResponse { Status = TopicEventResponse.Types.TopicEventResponseStatus.Success };
        }
        private TopicEventResponse HandlerDaprShowcaseEvent(DaprShowcaseEvent @event)
        {
            TopicEventResponse response = null;

            if (@event.EventType.Equals("daprshowcase-domain-updated", StringComparison.InvariantCultureIgnoreCase))
            {
                response = HandleEventData<OnDomainUpdatedEventData, OnDomainUpdatedEventHandler>(@event.Data);
            }
            else if (@event.EventType.Equals("daprshowcase-sequence-item-created", StringComparison.InvariantCultureIgnoreCase))
            {
                response = HandleEventData<OnSequenceItemCreatedEventData, OnSequenceItemCreatedEventHandler>(@event.Data);
            }

            return response;
        }
        
        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            var topicList = new ListTopicSubscriptionsResponse();

            topicList.Subscriptions.Add(new TopicSubscription { PubsubName = AUDITING_PUBSUB_NAME, Topic = AUDITING_PUBSUB_TOPIC });

            _logger.LogInformation($"Found {topicList.Subscriptions.Count} subscriptions");
            
            return Task.FromResult(topicList);
        }
        public override Task<TopicEventResponse> OnTopicEvent(TopicEventRequest request, ServerCallContext context)
        {
            TopicEventResponse response = null;

            try
            {
                var content = Encoding.UTF8.GetString(request.Data.ToByteArray());
                var @event = JsonConvert.DeserializeObject<DaprShowcaseEvent>(content);
                
                if (@event != null)
                {
                    response = HandlerDaprShowcaseEvent(@event);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
            }

            return Task.FromResult(response ?? new TopicEventResponse { Status = TopicEventResponse.Types.TopicEventResponseStatus.Drop });
        }
        public override Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            return base.OnInvoke(request, context);
        }
    }
}