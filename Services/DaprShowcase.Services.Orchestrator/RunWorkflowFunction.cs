using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using DaprShowcase.Services.Orchestrator.Activities;
using DaprShowcase.Services.Orchestrator.Application.Messages.Commands;
using DaprShowcase.Services.Orchestrator.Application.Messages.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DaprShowcase.Services.Orchestrator
{
    public static class RunWorkflowFunction
    {
        private static async Task WaitForExternalEventAsync(IDurableOrchestrationContext context)
        {
            var whenContinue = context.WaitForExternalEvent<object>(WorkflowEvent.ContinueWorkflowRequested.Name);
            var whenStop = context.WaitForExternalEvent<object>(WorkflowEvent.StopWorkflowRequested.Name);

            var result = await Task.WhenAny(whenContinue, whenStop);
        }
        private static async Task ScanBlobAsync(IDurableOrchestrationContext context, RunWorkflowCommand command)
        {
            var scanBlobActivityInput = new RunScanBlobActivityInput { WorkflowId = command.WorkflowId, CompanyId = command.CompanyId, FolderId = command.FolderId, FileName = command.FileName };
            await context.CallActivityAsync("ScanBlobActivity", scanBlobActivityInput);
        }
        private static async Task ZipBlobAsync(IDurableOrchestrationContext context, RunWorkflowCommand command)
        {
            var zipBlobActivityInput = new RunZipBlobActivityInput { WorkflowId = command.WorkflowId, CompanyId = command.CompanyId, FolderId = command.FolderId, FileName = command.FileName };
            await context.CallActivityAsync("ZipBlobActivity", zipBlobActivityInput);
        }
        private static async Task SendCallbackAsync(IDurableOrchestrationContext context, RunWorkflowCommand command)
        {
            var sendCallbackActivityInput = new RunSendCallbackActivityInput { WorkflowId = command.WorkflowId, CallbackUrl = command.CallbackUrl };
            await context.CallActivityAsync("SendCallbackActivity", sendCallbackActivityInput);
        }

        [FunctionName("RunWorkflow")]
        public static async Task RunWorkflow([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var command = context.GetInput<RunWorkflowCommand>();

            log.LogInformation($"Running workflow with ID {command.WorkflowId}");

            await ScanBlobAsync(context, command);
            await WaitForExternalEventAsync(context);

            await ZipBlobAsync(context, command);
            await WaitForExternalEventAsync(context);

            await SendCallbackAsync(context, command);
        }
    }
}