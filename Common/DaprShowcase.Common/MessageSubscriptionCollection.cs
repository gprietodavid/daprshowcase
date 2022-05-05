using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.AppCallback.Autogen.Grpc.v1;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Messages;

namespace DaprShowcase.Common
{
    public abstract class MessageSubscriptionCollection : Collection<IMessageSubscription>, IMessageSubscriptionCollection
    {
        private readonly IServiceProvider _serviceProvider;

        protected MessageSubscriptionCollection(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Add<TMessage, TMessageHandler>(string pubsubName, string topicName) 
            where TMessage : IMessage 
            where TMessageHandler : class, IMessageHandler<TMessage>
        {
            base.Add(new MessageSubscription<TMessage, TMessageHandler>(_serviceProvider, pubsubName, topicName));
        }

        public IEnumerable<TopicSubscription> GetSubscriptions()
        {
            foreach (var item in this.Items)
            {
                yield return new TopicSubscription { PubsubName = item.PubSubName, Topic = item.TopicName };
            }
        }

        public async Task<IMessageHandlerResult> HandleAsync(string pubsubame, string topic, string content)
        {
            var subscriptions = this.Items.Where(x => x.PubSubName.Equals(pubsubame, StringComparison.InvariantCultureIgnoreCase) && x.TopicName.Equals(topic, StringComparison.InvariantCultureIgnoreCase));
            if (!subscriptions.Any()) throw new InvalidOperationException($"No subscriptions available for topic {topic} on {pubsubame}!");
            if (subscriptions.Count() > 1) throw new InvalidOperationException($"Only one handler allowed per subscription!");

            return await subscriptions.First().HandleAsync(content);
        }
    }
}