using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Messages.Commands;

namespace DaprShowcase.Common.Application.Handlers.Commands
{
    public interface IWorkflowCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : IWorkflowCommand
    {
        string WorkflowId { get; set; }
    }
}