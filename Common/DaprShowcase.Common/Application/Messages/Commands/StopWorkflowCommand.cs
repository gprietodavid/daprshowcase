using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Application.Messages.Commands
{
    public class StopWorkflowCommand : WorkflowCommandBase
    {
        public override string Topic { get; } = "daprshowcase-stop-workflow";
        public string WorkerProcessOutput { get; set; }
    }
}