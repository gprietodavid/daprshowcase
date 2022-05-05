using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Services.Orchestrator.Adapters;
using DaprShowcase.Services.Orchestrator.Application.Messages.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DaprShowcase.Services.Orchestrator.Activities
{
    public class RunZipBlobActivityInput
    {
        public string WorkflowId { get; set; }
        public string CompanyId { get; set; }
        public string FolderId { get; set; }
        public string FileName { get; set; }
    }

    public class RunZipBlobActivity
    {
        private readonly IWorkflowPublisherAdapter _workflowPublisher;

        public RunZipBlobActivity(IWorkflowPublisherAdapter workflowPublisher)
        {
            _workflowPublisher = workflowPublisher;
        }

        [FunctionName("ZipBlobActivity")]
        public async Task ZipBlob([ActivityTrigger] RunZipBlobActivityInput input, ILogger log)
        {
            log.LogInformation($"Zipping blob for workflow with ID [{input.WorkflowId}]");
            var command = new ZipBlobCommand() { CompanyId = input.CompanyId, FolderId = input.FolderId, FileName = input.FileName, WorkflowId = input.WorkflowId };
            await _workflowPublisher.PublishCommandAsync(command);
        }
    }
}