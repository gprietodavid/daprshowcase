using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Application.Messages.Commands;

namespace DaprShowcase.Common.Adapters.WorkflowPublisher
{
    public abstract class WorkflowCommandPublisherAdapter : IWorkflowPublisherAdapter
    {
        private const string WORKFLOW_PUBSUB_NAME = "workflowpubsub";

        private readonly DaprShowcaseServiceBusPublisher _serviceBusPublisher;

        protected WorkflowCommandPublisherAdapter(DaprClient dapr)
        {
            _serviceBusPublisher = new DaprShowcaseServiceBusPublisher(dapr, WORKFLOW_PUBSUB_NAME);
        }

        public async Task PublishCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _serviceBusPublisher.PublishMessageAsync(command.Topic, command);
        }
    }
}