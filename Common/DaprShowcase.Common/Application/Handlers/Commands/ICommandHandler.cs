using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Messages.Commands;

namespace DaprShowcase.Common.Application.Handlers.Commands
{
    public interface ICommandHandler<TCommand> : IMessageHandler<TCommand>
        where TCommand : ICommand
    {
    }
}