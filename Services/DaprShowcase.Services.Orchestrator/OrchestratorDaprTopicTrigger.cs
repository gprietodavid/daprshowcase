using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using DaprShowcase.Services.Orchestrator.Application;
using DaprShowcase.Services.Orchestrator.Application.Messages;
using DaprShowcase.Services.Orchestrator.Application.Messages.Commands;
using DaprShowcase.Services.Orchestrator.Application.Messages.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DaprShowcase.Services.Orchestrator
{
    public static class OrchestratorDaprTopicTrigger
    {
        [FunctionName("Orchestrator_RunWorkflow_DaprTopicTrigger")]
        public static async Task RunWorkflowByDaprTopic([Application.Dapr.Triggers.DaprTopicTrigger("workflowpubsub", Topic = "daprshowcase-run-workflow")] CloudEvent @event, [DurableClient] IDurableOrchestrationClient client, ILogger log)
        {
            log.LogInformation($"Received message to run workflow");

            var data = @event.Data.ToString();
            var command = JsonConvert.DeserializeObject<RunWorkflowCommand>(data);
            command.WorkflowId = Guid.NewGuid().ToString();

            log.LogInformation($"Starting workflow for company [{command.CompanyId}], folder [{command.FolderId}] and filename [{command.FileName}]");

            string instanceId = await client.StartNewAsync("RunWorkflow", command.WorkflowId, command);

            log.LogInformation($"Started workflow with ID [{instanceId}]");
        }

        [FunctionName("Orchestrator_ContinueWorkflow_DaprTopicTrigger")]
        public static async Task ContinueWorkflowByDaprTopic([Application.Dapr.Triggers.DaprTopicTrigger("workflowpubsub", Topic = "daprshowcase-continue-workflow")] CloudEvent @event, [DurableClient] IDurableOrchestrationClient client, ILogger log)
        {
            log.LogInformation($"Received message to continue workflow");

            var data = @event.Data.ToString();
            var command = JsonConvert.DeserializeObject<ContinueWorkflowCommand>(data);
            var result = JsonConvert.DeserializeObject<WorkerProcessResult>(command.WorkerProcessOutput);

            log.LogInformation($"Continue workflow with ID {command.WorkflowId}");

            await client.RaiseEventAsync(command.WorkflowId, WorkflowEvent.ContinueWorkflowRequested.Name, result);
        }

        [FunctionName("Orchestrator_StopWorkflow_DaprTopicTrigger")]
        public static async Task StopWorkflowByDaprTopic([Application.Dapr.Triggers.DaprTopicTrigger("workflowpubsub", Topic = "daprshowcase-stop-workflow")] CloudEvent @event, [DurableClient] IDurableOrchestrationClient client, ILogger log)
        {
            log.LogInformation($"Received message to stop workflow");

            var data = @event.Data.ToString();
            var command = JsonConvert.DeserializeObject<ContinueWorkflowCommand>(data);
            var result = JsonConvert.DeserializeObject<WorkerProcessResult>(command.WorkerProcessOutput);

            log.LogInformation($"Stop workflow with ID {command.WorkflowId}");

            await client.RaiseEventAsync(command.WorkflowId, WorkflowEvent.StopWorkflowRequested.Name, result);
        }
    }
}