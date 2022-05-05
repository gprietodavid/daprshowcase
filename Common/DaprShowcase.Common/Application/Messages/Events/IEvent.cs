namespace DaprShowcase.Common.Application.Messages.Events
{
    public interface IEvent
    {
        string EventSource { get; }
        string EventType { get; }
        string Data { get; }
    }
}