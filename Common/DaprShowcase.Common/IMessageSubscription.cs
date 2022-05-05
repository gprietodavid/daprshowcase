using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Handlers;

namespace DaprShowcase.Common
{
    public interface IMessageSubscription
    {
        string PubSubName { get; }
        string TopicName { get; }
        Type MessageType { get; }
        Type HandlerType { get; }

        Task<IMessageHandlerResult> HandleAsync(string content);
    }
}