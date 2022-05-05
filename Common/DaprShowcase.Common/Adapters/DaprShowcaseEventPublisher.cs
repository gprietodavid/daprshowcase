using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Application.Messages.Events;

namespace DaprShowcase.Common.Adapters
{
    internal class DaprShowcaseEventPublisher
    {
        private readonly string _pubsubName;
        private readonly string _topicName;

        private readonly DaprClient _dapr;

        internal DaprShowcaseEventPublisher(DaprClient dapr, string pubsubName, string topicName)
        {
            _dapr = dapr;
            _pubsubName = pubsubName;
            _topicName = topicName;
        }

        public async Task PublishEventAsync(string eventName, object @event, string publishedBy = "")
        {
            var data = new DaprShowcaseEvent(eventName, @event, publishedBy);
            await _dapr.PublishEventAsync(_pubsubName, _topicName, data);
        }
    }
}