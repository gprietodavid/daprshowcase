using Dapr.Client;
using DaprShowcase.Common.Adapters.DataStorage;
using DaprShowcase.Services.EntitiesApi.Domain;

namespace DaprShowcase.Services.EntitiesApi.Adapters.CompanyStorage
{
    public class CompanyStorageAdapter : DaprStateStorageAdapterBase<Company>, ICompanyStorageAdapter
    {
        protected override string StateStoreName => "companystatestore";
        protected override string StateIndexName => "companyIds";

        public CompanyStorageAdapter(DaprClient dapr) : base(dapr)
        {
        }
    }
}