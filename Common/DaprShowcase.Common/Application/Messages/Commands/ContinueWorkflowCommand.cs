using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Application.Messages.Commands
{
    public class ContinueWorkflowCommand : WorkflowCommandBase
    {
        public override string Topic { get; } = "daprshowcase-continue-workflow";
        public string WorkerProcessOutput { get; set; }
    }
}