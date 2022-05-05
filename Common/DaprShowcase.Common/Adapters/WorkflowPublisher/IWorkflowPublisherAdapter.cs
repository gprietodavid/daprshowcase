using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Messages.Commands;

namespace DaprShowcase.Common.Adapters.WorkflowPublisher
{
    public interface IWorkflowPublisherAdapter
    {
        Task PublishCommandAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}