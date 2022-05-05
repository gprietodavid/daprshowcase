using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Application.Messages.Commands
{
    public class RunWorkflowCommand : WorkflowCommandBase
    {
        public override string Topic { get; } = "daprshowcase-run-workflow";
        public string CompanyId { get; set; }
        public string FolderId { get; set; }
        public string FileName { get; set; }
        public string CallbackUrl { get; set; }
    }
}