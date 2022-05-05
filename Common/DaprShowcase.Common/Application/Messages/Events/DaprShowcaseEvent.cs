using System;

namespace DaprShowcase.Common.Application.Messages.Events
{
    public sealed class DaprShowcaseEvent :  IEvent
    {
        private const int MAX_RETRIES = 5;

        public string EventType { get; }
        public string EventSource { get; }
        public string Data { get; }
        public string PublishedBy { get; }
        public DateTime PublishedOn { get; }
        public int Tries { get; private set; } = 0;
        public bool CanRetry => Tries <= MAX_RETRIES;
        public DateTime? HandledOn { get; private set; }
        public DateTime? NextHandleTryOn { get; private set; }
        public bool CanBeHandled => NextHandleTryOn == null || DateTime.UtcNow > NextHandleTryOn.Value;

        public DaprShowcaseEvent(string eventType, object data, string publishedBy = "null") : base()
        {
            EventType = eventType;
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            PublishedBy = publishedBy;
            PublishedOn = DateTime.UtcNow;
        }

        public void AddHandleTry()
        {
            var handledTime = DateTime.UtcNow;
            if (HandledOn != null) NextHandleTryOn = handledTime.AddSeconds(20);
            HandledOn = handledTime;
            Tries += 1;
        }
    }
}