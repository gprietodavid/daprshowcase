namespace DaprShowcase.Common.Application.Messages.Commands
{
    public class ScanBlobCommand : BlobCommandBase, IWorkflowCommand
    {
        public string WorkflowId { get; set; }
        public override string Topic { get; } = "daprshowcase-scan-file";
    }
}