using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Messages;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;

namespace DaprShowcase.Common.Application.Handlers
{
    public abstract class MessageHandlerBase<TMessage> : IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        public class IgnoreStreamsResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(
                MemberInfo member,
                MemberSerialization memberSerialization
            )
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);
                if (typeof(Stream).IsAssignableFrom(property.PropertyType))
                {
                    property.Ignored = true;
                }
                return property;
            }
        }

        private readonly TelemetryClient _telemetryClient;

        protected MessageHandlerBase(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        protected virtual IMessageHandlerResult Fail(string[] errors)
        {
            return new MessageHandlerResult(false, errors);
        }
        protected virtual IMessageHandlerResult Ok(params string[] messages)
        {
            return new MessageHandlerResult(true, messages);
        }

        protected abstract Task<IMessageHandlerResult> DoHandleAsync(TMessage cmd);

        public virtual async Task<IMessageHandlerResult> HandleAsync(TMessage msg)
        {
            var messageName = typeof(TMessage).Name;
            var handlerName = GetType().Name;

            using (_telemetryClient.StartOperation<RequestTelemetry>(handlerName, msg.Id))
            {
                _telemetryClient.TrackEvent(messageName, new Dictionary<string, string> { { "CorrelationId", msg.Id } });

                try
                {
                    return await DoHandleAsync(msg);
                }
                catch (Exception ex)
                {
                    _telemetryClient.TrackException(ex, new Dictionary<string, string> { { "Operation", handlerName }, { "Request", JsonConvert.SerializeObject(msg, new JsonSerializerSettings { ContractResolver = new IgnoreStreamsResolver() }) } });
                    throw;
                }
            }
        }

        public virtual async Task<IMessageHandlerResult> HandleWithRetryAsync(TMessage msg, int retryCount, Func<int, TimeSpan> sleepDurationFunc, Action<Exception> onExceptionAction)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(retryCount, sleepDurationFunc);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    return await HandleAsync(msg);
                }
                catch (Exception ex)
                {
                    onExceptionAction?.Invoke(ex);
                    throw;
                }
            }).ConfigureAwait(false);
        }
    }
}