using System;
using System.Collections.Generic;
using System.Text;

namespace DaprShowcase.Services.Orchestrator.Application.Messages.Events
{
    public sealed class WorkflowEvent
    {
        public string Name { get; }

        private WorkflowEvent(string name)
        {
            Name = name;
        }

        public static WorkflowEvent ContinueWorkflowRequested => new WorkflowEvent("daprshowcase-continue-workflow-requested-event");
        public static WorkflowEvent StopWorkflowRequested => new WorkflowEvent("daprshowcase-stop-workflow-requested-event");
    }
}