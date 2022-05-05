using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Handlers.Commands;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DaprShowcase.Common
{
    public abstract class WorkerProcessServiceBase: AppCallback.AppCallbackBase
    {
        private const string WORKFLOW_PUBSUB_NAME = "workflowpubsub";

        private readonly ILogger<WorkerProcessServiceBase> _logger;
        private readonly IMessageSubscriptionCollection _subscriptions;

        protected WorkerProcessServiceBase(ILogger<WorkerProcessServiceBase> logger, IMessageSubscriptionCollection subscriptions)
        {
            _logger = logger;
            _subscriptions = subscriptions;
        }

        protected virtual bool TryHandleCommand(string pubsubName, string topic, string content, out IMessageHandlerResult result)
        {
            result = _subscriptions.HandleAsync(pubsubName, topic, content).Result;
            return result.IsValid;
        }
        protected TopicEventResponse HandleCommand(string pubsubName, string topic, string content)
        {
            return TryHandleCommand(pubsubName, topic, content, out var result) ? new TopicEventResponse { Status = TopicEventResponse.Types.TopicEventResponseStatus.Drop } : new TopicEventResponse { Status = TopicEventResponse.Types.TopicEventResponseStatus.Success };
        }

        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            var topicList = new ListTopicSubscriptionsResponse();
            var subscriptions = _subscriptions.GetSubscriptions();

            foreach (var subscription in subscriptions)
            {
                topicList.Subscriptions.Add(subscription);
            }

            _logger.LogInformation($"Found {topicList.Subscriptions.Count} subscriptions");

            return Task.FromResult(topicList);
        }
        public override Task<TopicEventResponse> OnTopicEvent(TopicEventRequest request, ServerCallContext context)
        {
            TopicEventResponse response = null;

            try
            {
                var content = Encoding.UTF8.GetString(request.Data.ToByteArray());
                response = HandleCommand(request.PubsubName, request.Topic, content);
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