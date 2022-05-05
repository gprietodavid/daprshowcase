namespace DaprShowcase.Services.Orchestrator.Application.Messages.Commands
{
    public class ContinueWorkflowCommand : CommandBase
    {
        public override string Topic { get; } = "daprshowcase-continue-workflow";
        public string WorkflowId { get; set; }
        public string WorkerProcessOutput { get; set; }
    }
}