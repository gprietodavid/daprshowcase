using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Services.Orchestrator.Adapters;
using DaprShowcase.Services.Orchestrator.Application.Messages.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DaprShowcase.Services.Orchestrator.Activities
{
    public class RunSendCallbackActivityInput
    {
        public string WorkflowId { get; set; }
        public string CallbackUrl { get; set; }
        public object Summary { get; set; }
    }

    public class RunSendCallbackActivity
    {
        private readonly HttpClient _http;

        public RunSendCallbackActivity(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
        }

        [FunctionName("SendCallbackActivity")]
        public async Task SendCallback([ActivityTrigger] RunSendCallbackActivityInput input, ILogger log)
        {
            log.LogInformation($"Send callback for workflow with ID [{input.WorkflowId}] to {input.CallbackUrl}");
            var content = JsonContent.Create(input.Summary);
            var response = await _http.PostAsync(input.CallbackUrl, content);
        }
    }
}