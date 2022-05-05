namespace DaprShowcase.Common.Application.Messages.Commands
{
    public class ZipBlobCommand : BlobCommandBase, IWorkflowCommand
    {
        public string WorkflowId { get; set; }
        public override string Topic { get; } = "daprshowcase-zip-file";
    }
}