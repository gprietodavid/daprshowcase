using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Services.Orchestrator.Application;
using DaprShowcase.Services.Orchestrator.Application.Messages;
using DaprShowcase.Services.Orchestrator.Application.Messages.Commands;
using DaprShowcase.Services.Orchestrator.Application.Messages.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DaprShowcase.Services.Orchestrator
{
    public static class OrchestratorHttpTrigger
    {
        [FunctionName("Orchestrator_RunWorkflow_HttpTrigger")]
        public static async Task RunWorkflowByHttpRequest([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage request, [DurableClient] IDurableOrchestrationClient client, ILogger log)
        {
            log.LogInformation($"Received request to run workflow");

            var data = await request.Content.ReadAsStringAsync();
            var command = JsonConvert.DeserializeObject<RunWorkflowCommand>(data);

            log.LogInformation($"Starting workflow for company [{command.CompanyId}], folder [{command.FolderId}] and filename [{command.FileName}]");

            string instanceId = await client.StartNewAsync("RunWorkflow", command.WorkflowId, command);

            log.LogInformation($"Started workflow with ID [{instanceId}]");
        }

        [FunctionName("Orchestrator_ContinueWorkflow_HttpTrigger")]
        public static async Task ContinueWorkflowByHttpRequest([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage request, [DurableClient] IDurableOrchestrationClient client, ILogger log)
        {
            log.LogInformation($"Received message to continue workflow");

            var data = await request.Content.ReadAsStringAsync();
            var command = JsonConvert.DeserializeObject<ContinueWorkflowCommand>(data);
            var result = JsonConvert.DeserializeObject<WorkerProcessResult>(command.WorkerProcessOutput);

            log.LogInformation($"Continue workflow with ID {command.WorkflowId}");

            await client.RaiseEventAsync(command.WorkflowId, WorkflowEvent.ContinueWorkflowRequested.Name, result);
        }

        [FunctionName("Orchestrator_StopWorkflow_HttpTrigger")]
        public static async Task StopWorkflowByHttpRequest([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage request, [DurableClient] IDurableOrchestrationClient client, ILogger log)
        {
            log.LogInformation($"Received message to stop workflow");

            var data = await request.Content.ReadAsStringAsync();
            var command = JsonConvert.DeserializeObject<ContinueWorkflowCommand>(data);
            var result = JsonConvert.DeserializeObject<WorkerProcessResult>(command.WorkerProcessOutput);

            log.LogInformation($"Stop workflow with ID {command.WorkflowId}");

            await client.RaiseEventAsync(command.WorkflowId, WorkflowEvent.StopWorkflowRequested.Name, result);
        }
    }
}