namespace DaprShowcase.Services.Orchestrator.Application.Messages.Commands
{
    public interface ICommand : IMessage
    {
        string Topic { get; }
    }
}