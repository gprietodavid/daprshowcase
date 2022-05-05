using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;

namespace DaprShowcase.Common.Extensions
{
    public static class DaprExtensions
    {
        public static async Task<TResponseContent> TryInvokeMethodAsync<TResponseContent>(this DaprClient dapr, HttpMethod method, string appId, string methodName) where TResponseContent : class
        {
            var request = dapr.CreateInvokeMethodRequest(method, appId, methodName);
            var response = await dapr.InvokeMethodWithResponseAsync(request);

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content)) return null;

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponseContent>(content);
        }
    }
}