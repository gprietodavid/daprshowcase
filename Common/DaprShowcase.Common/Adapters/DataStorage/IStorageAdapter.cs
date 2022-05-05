using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DaprShowcase.Common.Domain.Entities;

namespace DaprShowcase.Common.Adapters.DataStorage
{
    public interface IStorageAdapter<TValue> where TValue : class, IEntity
    {
        Task<IEnumerable<TValue>> GetAllAsync();
        Task<TValue> GetAsync(Guid id);
        Task<Guid> AddAsync(TValue entity);
        Task UpdateAsync(TValue entity);
        Task DeleteAsync(Guid id);
    }
}