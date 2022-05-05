using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Handlers.Events;
using DaprShowcase.Common.Application.Messages.Events;
using DaprShowcase.Services.AuditingWorker.Adapters;
using DaprShowcase.Services.AuditingWorker.Domain.Entities;
using Microsoft.ApplicationInsights;

namespace DaprShowcase.Services.AuditingWorker.Application.Handlers.Events
{
    public class OnDomainUpdatedEventHandler : EventHandlerBase<OnDomainUpdatedEventData>
    {
        private readonly IAuditRecordStorageAdapter _auditRecordStorage;

        public OnDomainUpdatedEventHandler(TelemetryClient telemetryClient, IAuditRecordStorageAdapter auditRecordStorage) : base(telemetryClient)
        {
            _auditRecordStorage = auditRecordStorage;
        }

        protected override async Task<IMessageHandlerResult> DoHandleAsync(OnDomainUpdatedEventData cmd)
        {
            var auditRecord = new AuditRecord { EntityId = cmd.EntityId, EntityType = cmd.EntityType, Action = cmd.Action, DeltaJson = cmd.DeltaJson, MadeBy = "", MadeOn = DateTime.UtcNow };
            await _auditRecordStorage.AddAsync(auditRecord);

            return Ok();
        }
    }
}