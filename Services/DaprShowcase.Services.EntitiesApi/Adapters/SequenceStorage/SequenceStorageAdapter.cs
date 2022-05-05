using Dapr.Client;
using DaprShowcase.Common.Adapters.DataStorage;
using DaprShowcase.Services.EntitiesApi.Domain;

namespace DaprShowcase.Services.EntitiesApi.Adapters.SequenceStorage
{
    public class SequenceStorageAdapter : DaprStateStorageAdapterBase<SequenceItem>, ISequenceStorageAdapter
    {
        protected override string StateStoreName => "sequencestatestore";
        protected override string StateIndexName => "sequenceItemIds";

        public SequenceStorageAdapter(DaprClient dapr) : base(dapr)
        {
        }
    }
}