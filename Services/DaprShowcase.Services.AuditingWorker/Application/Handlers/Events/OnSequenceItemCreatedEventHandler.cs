using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Handlers.Events;
using DaprShowcase.Common.Application.Messages.Events;
using DaprShowcase.Services.AuditingWorker.Adapters;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace DaprShowcase.Services.AuditingWorker.Application.Handlers.Events
{
    public class OnSequenceItemCreatedEventHandler : EventHandlerBase<OnSequenceItemCreatedEventData>
    {
        private readonly ILogger<OnSequenceItemCreatedEventHandler> _logger;
        private readonly ISequenceAdapter _sequence;

        public OnSequenceItemCreatedEventHandler(TelemetryClient telemetryClient, ILogger<OnSequenceItemCreatedEventHandler> logger, ISequenceAdapter sequence) : base(telemetryClient)
        {
            _logger = logger;
            _sequence = sequence;
        }

        protected override async Task<IMessageHandlerResult> DoHandleAsync(OnSequenceItemCreatedEventData cmd)
        {
            _logger.LogInformation($"Handling sequence event, value {cmd.Value}");

            if (_sequence.TryAddValue(cmd.Value))
            {
                _logger.LogInformation($"Value added successfully, items on sequence: {_sequence.GetSequence()}");
            }
            else
            {
                throw new System.InvalidOperationException($"Can't add value {cmd.Value} to sequence [{_sequence.GetSequence()}]!");
            }

            return Ok();
        }
    }
}