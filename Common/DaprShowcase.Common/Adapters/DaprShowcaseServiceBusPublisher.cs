using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;

namespace DaprShowcase.Common.Adapters
{
    public class DaprShowcaseServiceBusPublisher
    {
        private readonly string _pubsubName;

        private readonly DaprClient _dapr;

        internal DaprShowcaseServiceBusPublisher(DaprClient dapr, string pubsubName)
        {
            _dapr = dapr;
            _pubsubName = pubsubName;
        }

        public async Task PublishMessageAsync<TData>(string topic, TData data)
        {
            await _dapr.PublishEventAsync(_pubsubName, topic, data);
        }
    }
}