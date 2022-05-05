namespace DaprShowcase.Services.Orchestrator.Application.Messages.Commands
{
    public sealed class RunWorkflowCommand : CommandBase
    {
        public override string Topic { get; } = "daprshowcase-run-workflow";
        public string WorkflowId { get; set; }
        public string CompanyId { get; set; }
        public string FolderId { get; set; }
        public string FileName { get; set; }
        public string CallbackUrl { get; set; }
    }
}