using DaprShowcase.Common.Adapters.DataStorage;
using DaprShowcase.Services.EntitiesApi.Domain;

namespace DaprShowcase.Services.EntitiesApi.Adapters.SequenceStorage
{
    public interface ISequenceStorageAdapter : IStorageAdapter<SequenceItem>
    {
    }
}