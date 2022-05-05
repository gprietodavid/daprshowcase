using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Domain.Entities;

namespace DaprShowcase.Common.Adapters.DataStorage
{
    public abstract class DaprStateStorageAdapterBase<TValue> : IStorageAdapter<TValue>
        where TValue : class, IEntity
    {
        protected readonly DaprClient _dapr;

        protected abstract string StateStoreName { get; }
        protected abstract string StateIndexName { get; }

        protected DaprStateStorageAdapterBase(DaprClient dapr)
        {
            _dapr = dapr;
        }

        protected virtual async Task<List<string>> GetIdsAsync()
        {
            var ids = await _dapr.GetStateAsync<List<string>>(StateStoreName, StateIndexName);
            return ids ?? new List<string>();
        }
        protected virtual async Task AddIdAsync(Guid id)
        {
            var ids = await GetIdsAsync();
            ids.Add(id.ToString());
            await _dapr.SaveStateAsync(StateStoreName, StateIndexName, ids.ToArray());
        }
        protected virtual async Task DeleteIdAsync(Guid id)
        {
            var ids = await GetIdsAsync();
            ids.Remove(id.ToString());
            await _dapr.SaveStateAsync(StateStoreName, StateIndexName, ids.ToArray());
        }

        public virtual async Task<IEnumerable<TValue>> GetAllAsync()
        {
            var ids = await GetIdsAsync();
            if (ids == null || !ids.Any()) return new List<TValue>();
            var items = await _dapr.GetBulkStateAsync(StateStoreName, ids, null);
            var entities = items.Select(x => Newtonsoft.Json.JsonConvert.DeserializeObject<TValue>(x.Value));

            return entities;
        }
        public virtual async Task<TValue> GetAsync(Guid id)
        {
            return await _dapr.GetStateAsync<TValue>(StateStoreName, id.ToString());
        }
        public virtual async Task<Guid> AddAsync(TValue entity)
        {
            await _dapr.SaveStateAsync(StateStoreName, entity.Id.ToString(), entity);
            await AddIdAsync(entity.Id);
            
            return entity.Id;
        }
        public virtual async Task UpdateAsync(TValue entity)
        {
            await _dapr.SaveStateAsync(StateStoreName, entity.Id.ToString(), entity);
        }
        public virtual async Task DeleteAsync(Guid id)
        {
            await DeleteIdAsync(id);
            await _dapr.DeleteStateAsync(StateStoreName, id.ToString());
        }
    }
}