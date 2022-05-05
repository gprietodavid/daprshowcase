using DaprShowcase.Common.Application.Messages.Events;
using Microsoft.ApplicationInsights;

namespace DaprShowcase.Common.Application.Handlers.Events
{
    public abstract class EventHandlerBase<TEvent> : MessageHandlerBase<TEvent>, IEventHandler<TEvent>
        where TEvent : IEventData
    {
        protected EventHandlerBase(TelemetryClient telemetryClient) : base(telemetryClient)
        {
        }
    }
}