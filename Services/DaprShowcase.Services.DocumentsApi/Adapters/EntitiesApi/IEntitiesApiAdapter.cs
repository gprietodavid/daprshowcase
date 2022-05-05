using System;
using System.Threading.Tasks;
using DaprShowcase.Services.DocumentsApi.Domain;

namespace DaprShowcase.Services.DocumentsApi.Adapters.EntitiesApi
{
    public interface IEntitiesApiAdapter
    {
        Task<Company> GetCompanyAsync(Guid companyId);
    }
}