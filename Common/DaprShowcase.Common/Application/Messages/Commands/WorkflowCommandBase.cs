using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Application.Messages.Commands
{
    public abstract class WorkflowCommandBase : CommandBase, IWorkflowCommand
    {
        public string WorkflowId { get; set; }
    }
}