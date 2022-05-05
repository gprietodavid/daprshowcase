using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Application.Messages.Commands;

namespace DaprShowcase.Common.Adapters.OrchestratorPublisher
{
    public class OrchestratorPublisherAdapter : IOrchestratorPublisherAdapter
    {
        private const string WORKFLOW_PUBSUB_NAME = "workflowpubsub";

        private readonly DaprShowcaseServiceBusPublisher _serviceBusPublisher;

        public OrchestratorPublisherAdapter(DaprClient dapr)
        {
            _serviceBusPublisher = new DaprShowcaseServiceBusPublisher(dapr, WORKFLOW_PUBSUB_NAME);
        }

        public async Task PublishRunWorkflowCommandAsync(string workflowId, string companyId, string folderId, string filename, string callbackUrl)
        {
            var command = new RunWorkflowCommand() { CompanyId = companyId, FolderId = folderId, FileName = filename, CallbackUrl = callbackUrl };
            await _serviceBusPublisher.PublishMessageAsync(command.Topic, command);
        }

        public async Task PublishContinueWorkflowCommandAsync<TData>(string workflowId, TData data = default(TData)) where TData : class
        {
            var command = new ContinueWorkflowCommand() { WorkflowId = workflowId, WorkerProcessOutput = Newtonsoft.Json.JsonConvert.SerializeObject(data) };
            await _serviceBusPublisher.PublishMessageAsync(command.Topic, command);
        }

        public async Task PublishStopWorkflowCommandAsync<TData>(string workflowId, TData data = default(TData)) where TData : class
        {
            var command = new StopWorkflowCommand() { WorkflowId = workflowId, WorkerProcessOutput = Newtonsoft.Json.JsonConvert.SerializeObject(data) };
            await _serviceBusPublisher.PublishMessageAsync(command.Topic, command);
        }
    }
}
