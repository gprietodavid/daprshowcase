using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Adapters.DataStorage;
using DaprShowcase.Services.DocumentsApi.Adapters.EntitiesApi;
using DaprShowcase.Services.DocumentsApi.Domain;

namespace DaprShowcase.Services.DocumentsApi.Adapters.CompanyFolderStorage
{
    public class CompanyFolderStorageAdapter : DaprStateStorageAdapterBase<CompanyFolder>, ICompanyFolderStorageAdapter
    {
        private readonly IEntitiesApiAdapter _entitiesApi;

        protected override string StateStoreName => "companyfolderstatestore";
        protected override string StateIndexName => "companyFolderIds";

        public CompanyFolderStorageAdapter(DaprClient dapr, IEntitiesApiAdapter entitiesApi) : base(dapr)
        {
            _entitiesApi = entitiesApi;
        }

        public async Task<IEnumerable<CompanyFolder>> GetAllAsync(Guid companyId)
        {
            var companyFolders = await GetAllAsync();
            return companyFolders == null ? new List<CompanyFolder>() : companyFolders.Where(x => x.CompanyId == companyId).ToList();
        }
        //public override async Task<Guid> AddAsync(CompanyFolder entity)
        //{
        //    var company = await _entitiesApi.GetCompanyAsync(entity.CompanyId);
        //    if (company == null) throw new InvalidOperationException($"Company with ID {entity.CompanyId} does not exist!");

        //    var companyFolders =  await GetAllAsync(entity.CompanyId);
        //    if (companyFolders.Any(x => x.Name.Equals(entity.Name, StringComparison.InvariantCultureIgnoreCase))) throw new InvalidOperationException($"Company Folder with name '{entity.Name}' already exists!");

        //    return await base.AddAsync(entity);
        //}
    }
}