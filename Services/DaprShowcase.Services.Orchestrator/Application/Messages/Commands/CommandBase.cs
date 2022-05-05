namespace DaprShowcase.Services.Orchestrator.Application.Messages.Commands
{
    public abstract class CommandBase : MessageBase, ICommand
    {
        protected override string Prefix { get; } = "cmd";
        public abstract string Topic { get; }
    }
}