using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.AppCallback.Autogen.Grpc.v1;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Messages;

namespace DaprShowcase.Common
{
    public interface IMessageSubscriptionCollection : ICollection<IMessageSubscription>
    {
        void Add<TMessage, TMessageHandler>(string pubsubName, string topicName) 
            where TMessage : IMessage
            where TMessageHandler : class, IMessageHandler<TMessage>;

        IEnumerable<TopicSubscription> GetSubscriptions();

        Task<IMessageHandlerResult> HandleAsync(string pubsubame, string topic, string content);
    }
}