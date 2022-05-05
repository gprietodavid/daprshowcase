using System;
using Newtonsoft.Json;

namespace DaprShowcase.Common.Application.Messages.Events
{
    public class OnDomainUpdatedEventData : EventDataBase
    {
        public sealed class DomainUpdatedAction 
        {
            public string Name { get; }

            private DomainUpdatedAction(string name)
            {
                Name = name;
            }

            public static DomainUpdatedAction Created => new DomainUpdatedAction("created");
            public static DomainUpdatedAction Updated => new DomainUpdatedAction("updated");
            public static DomainUpdatedAction Deleted => new DomainUpdatedAction("deleted");
        }
        
        public string EntityType { get; }
        public string EntityId { get; }
        public string Action { get; }
        public string DeltaJson { get; }

        [JsonConstructor]
        public OnDomainUpdatedEventData(string entityId, string entityType, string action, string deltaJson)
        {
            EntityId = entityId;
            EntityType = entityType;
            Action = action;
            DeltaJson = deltaJson;
        }
        public OnDomainUpdatedEventData(object entityId, Type entityType, DomainUpdatedAction action, string deltaJson) : this(entityId.ToString(), entityType.Name, action.Name, deltaJson)
        {
        }
        
    }
}