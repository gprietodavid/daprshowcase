using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Messages;
using Newtonsoft.Json;

namespace DaprShowcase.Common
{
    public class MessageSubscription<TMessage, TMessageHandler> : IMessageSubscription
        where TMessage : IMessage
        where TMessageHandler : class, IMessageHandler<TMessage>
    {
        private readonly IServiceProvider _serviceProvider;

        public string PubSubName { get; }
        public string TopicName { get; }
        public Type MessageType => typeof(TMessage);
        public Type HandlerType => typeof(TMessageHandler);

        public MessageSubscription(IServiceProvider serviceProvider, string pubsubName, string topicName)
        {
            _serviceProvider = serviceProvider;
            PubSubName = pubsubName;
            TopicName = topicName;
        }

        public async Task<IMessageHandlerResult> HandleAsync(string content)
        {
            var message = JsonConvert.DeserializeObject<TMessage>(content);
            var handler = _serviceProvider.GetService(typeof(TMessageHandler)) as TMessageHandler;

            return await handler?.HandleAsync(message);
        }
    }
}