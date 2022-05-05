using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Extensions;
using DaprShowcase.Services.DocumentsApi.Domain;

namespace DaprShowcase.Services.DocumentsApi.Adapters.EntitiesApi
{
    public class EntitiesApiAdapter : IEntitiesApiAdapter
    {
        private const string ENTITIES_API_APP_ID = "daprshowcase-services-entitiesapi";

        private readonly DaprClient _dapr;

        public EntitiesApiAdapter(DaprClient dapr)
        {
            _dapr = dapr;
        }
        
        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            //return await _dapr.TryInvokeMethodAsync<Company>(HttpMethod.Get, ENTITIES_API_APP_ID, $"api/companies/{companyId}");
            return await _dapr.InvokeMethodAsync<Company>(HttpMethod.Get, ENTITIES_API_APP_ID, $"api/companies/{companyId}");
        }
    }
}