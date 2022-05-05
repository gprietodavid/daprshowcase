using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Services.Orchestrator.Application.Messages.Commands;

namespace DaprShowcase.Services.Orchestrator.Adapters
{
    public class WorkflowPublisherAdapter : IWorkflowPublisherAdapter
    {
        private const string WORKFLOW_PUBSUB_NAME = "workflowpubsub";

        private readonly DaprClient _dapr;

        public WorkflowPublisherAdapter(DaprClient dapr)
        {
            _dapr = dapr;
        }

        public async Task PublishCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _dapr.PublishEventAsync(WORKFLOW_PUBSUB_NAME, command.Topic, command);
        }
    }
}