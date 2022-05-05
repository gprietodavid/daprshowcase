namespace DaprShowcase.Common.Application.Messages.Events
{
    public abstract class EventDataBase : MessageBase, IEventData
    {
        protected override string Prefix => "evnt";
    }
}