namespace DaprShowcase.Services.Orchestrator.Application.Messages.Commands
{
    public class ScanBlobCommand : BlobCommandBase
    {
        public string WorkflowId { get; set; }
        public override string Topic { get; } = "daprshowcase-scan-file";
    }
}