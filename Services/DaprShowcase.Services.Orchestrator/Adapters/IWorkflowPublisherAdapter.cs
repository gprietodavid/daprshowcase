using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Services.Orchestrator.Application.Messages.Commands;

namespace DaprShowcase.Services.Orchestrator.Adapters
{
    public interface IWorkflowPublisherAdapter
    {
        Task PublishCommandAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}