namespace DaprShowcase.Services.Orchestrator.Application.Messages.Commands
{
    public class ZipBlobCommand : BlobCommandBase
    {
        public string WorkflowId { get; set; }
        public override string Topic { get; } = "daprshowcase-zip-file";
    }
}