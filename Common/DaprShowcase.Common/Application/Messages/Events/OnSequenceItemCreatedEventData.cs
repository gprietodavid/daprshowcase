namespace DaprShowcase.Common.Application.Messages.Events
{
    public class OnSequenceItemCreatedEventData : EventDataBase
    {
        public int Value { get; }

        public OnSequenceItemCreatedEventData(int value)
        {
            Value = value;
        }
    }
}