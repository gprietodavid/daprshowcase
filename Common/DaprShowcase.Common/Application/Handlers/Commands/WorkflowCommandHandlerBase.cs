using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Adapters.OrchestratorPublisher;
using DaprShowcase.Common.Application.Messages;
using DaprShowcase.Common.Application.Messages.Commands;
using Microsoft.ApplicationInsights;

namespace DaprShowcase.Common.Application.Handlers.Commands
{
    public abstract class WorkflowCommandHandlerBase<TCommand> : CommandHandlerBase<TCommand>, IWorkflowCommandHandler<TCommand>
        where TCommand : IWorkflowCommand
    {
        private readonly IOrchestratorPublisherAdapter _orchestratorPublisher;

        public string WorkflowId { get; set; }

        protected WorkflowCommandHandlerBase(TelemetryClient telemetryClient, IOrchestratorPublisherAdapter orchestratorPublisher) : base(telemetryClient)
        {
            _orchestratorPublisher = orchestratorPublisher;
        }

        private async Task PublishCommandToOrchestratorAsync(string workflowId, IMessageHandlerResult handlerResult)
        {
            if (handlerResult.IsValid)
            {
                var result = new WorkerProcessResult { HasErrors = false, JsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(handlerResult.Result) };
                await _orchestratorPublisher.PublishContinueWorkflowCommandAsync(workflowId, result);
            }
            else
            {
                var result = new WorkerProcessResult { HasErrors = true, Messages = handlerResult.Messages.ToArray(), JsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(handlerResult.Result) };
                await _orchestratorPublisher.PublishStopWorkflowCommandAsync(workflowId, result);
            }
        }

        public override async Task<IMessageHandlerResult> HandleAsync(TCommand msg)
        {
            WorkflowId = msg.WorkflowId;

            var handlerResult = await base.HandleAsync(msg);
            await PublishCommandToOrchestratorAsync(msg.WorkflowId, handlerResult);

            return handlerResult;
        }

        public override async Task<IMessageHandlerResult> HandleWithRetryAsync(TCommand msg, int retryCount, Func<int, TimeSpan> sleepDurationFunc, Action<Exception> onExceptionAction)
        {
            WorkflowId = msg.WorkflowId;

            var handlerResult = await base.HandleWithRetryAsync(msg, retryCount, sleepDurationFunc, onExceptionAction);
            await PublishCommandToOrchestratorAsync(msg.WorkflowId, handlerResult);

            return handlerResult;
        }
    }
}