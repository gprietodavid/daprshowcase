using DaprShowcase.Common.Application.Messages.Events;

namespace DaprShowcase.Common.Application.Handlers.Events
{
    public interface IEventHandler<TEventData> : IMessageHandler<TEventData>
        where TEventData : IEventData
    {
    }
}