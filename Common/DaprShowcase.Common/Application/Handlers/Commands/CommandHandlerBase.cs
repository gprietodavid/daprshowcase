using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Messages.Commands;
using Microsoft.ApplicationInsights;

namespace DaprShowcase.Common.Application.Handlers.Commands
{
    public abstract class CommandHandlerBase<TCommand> : MessageHandlerBase<TCommand>
        where TCommand : ICommand
    {
        protected CommandHandlerBase(TelemetryClient telemetryClient) : base(telemetryClient)
        {
        }
    }
}