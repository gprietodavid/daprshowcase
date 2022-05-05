using System.Threading.Tasks;
using DaprShowcase.Common.Domain.Entities;

namespace DaprShowcase.Common.Adapters.AuditingPublisher
{
    public interface IAuditingPublisherAdapter
    {
        Task PublishDomainEntityCreatedAsync<TEntity>(TEntity entity, string raisedBy) where TEntity: IEntity;
        Task PublishSequenceItemCreatedAsync(int value, string raisedBy);
    }
}