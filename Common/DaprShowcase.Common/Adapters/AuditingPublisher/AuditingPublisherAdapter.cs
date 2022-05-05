using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Application.Messages.Events;
using DaprShowcase.Common.Domain.Entities;

namespace DaprShowcase.Common.Adapters.AuditingPublisher
{
    public class AuditingPublisherAdapter : IAuditingPublisherAdapter
    {
        private const string AUDIT_PUBSUB_NAME = "auditingpubsub";
        private const string TOPIC_NAME = "daprshowcase";
        private const string DEFAULT_USER_NAME = "system";
        private const string DOMAIN_UPDATED_NAME = "daprshowcase-domain-updated";
        private const string SEQUENCE_ITEM_CREATED_NAME = "daprshowcase-sequence-item-created";

        private readonly DaprShowcaseEventPublisher _eventPublisher;

        public AuditingPublisherAdapter(DaprClient dapr)
        {
            _eventPublisher = new DaprShowcaseEventPublisher(dapr, AUDIT_PUBSUB_NAME, TOPIC_NAME);
        }

        public async Task PublishDomainEntityCreatedAsync<TEntity>(TEntity entity, string raisedBy = DEFAULT_USER_NAME) where TEntity : IEntity
        {
            var data = new OnDomainUpdatedEventData(entity.Id.ToString(), typeof(TEntity), OnDomainUpdatedEventData.DomainUpdatedAction.Created, Newtonsoft.Json.JsonConvert.SerializeObject(entity));
            await _eventPublisher.PublishEventAsync(DOMAIN_UPDATED_NAME, data, raisedBy);
        }
        public async Task PublishSequenceItemCreatedAsync(int value, string raisedBy)
        {
            var data = new OnSequenceItemCreatedEventData(value);
            await _eventPublisher.PublishEventAsync(SEQUENCE_ITEM_CREATED_NAME, data, raisedBy);
        }
    }
}