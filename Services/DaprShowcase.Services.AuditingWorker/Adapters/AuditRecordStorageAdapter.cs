using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Adapters;
using DaprShowcase.Common.Adapters.DataStorage;
using DaprShowcase.Services.AuditingWorker.Domain.Entities;

namespace DaprShowcase.Services.AuditingWorker.Adapters
{
    public class AuditRecordStorageAdapter : DaprStateStorageAdapterBase<AuditRecord>, IAuditRecordStorageAdapter
    {
        protected override string StateStoreName => "auditstatestore";
        protected override string StateIndexName => "auditRecordIds";

        public AuditRecordStorageAdapter(DaprClient dapr) : base(dapr)
        {
        }
    }
}