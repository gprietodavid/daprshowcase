using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Services.Orchestrator.Adapters;
using DaprShowcase.Services.Orchestrator.Application.Messages.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DaprShowcase.Services.Orchestrator.Activities
{
    public class RunScanBlobActivityInput
    {
        public string WorkflowId { get; set; }
        public string CompanyId { get; set; }
        public string FolderId { get; set; }
        public string FileName { get; set; }
    }

    public class RunScanBlobActivity
    {
        private readonly IWorkflowPublisherAdapter _workflowPublisher;

        public RunScanBlobActivity(IWorkflowPublisherAdapter workflowPublisher)
        {
            _workflowPublisher = workflowPublisher;
        }

        [FunctionName("ScanBlobActivity")]
        public async Task ScanBlob([ActivityTrigger] RunScanBlobActivityInput input, ILogger log)
        {
            log.LogInformation($"Scanning blob for workflow with ID [{input.WorkflowId}]");
            var command = new ScanBlobCommand { CompanyId = input.CompanyId, FolderId = input.FolderId, FileName = input.FileName, WorkflowId = input.WorkflowId };
            await _workflowPublisher.PublishCommandAsync(command);
        }
    }
}