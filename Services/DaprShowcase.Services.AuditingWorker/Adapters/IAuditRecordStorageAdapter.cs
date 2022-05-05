using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Adapters;
using DaprShowcase.Common.Adapters.DataStorage;
using DaprShowcase.Services.AuditingWorker.Domain.Entities;

namespace DaprShowcase.Services.AuditingWorker.Adapters
{
    public interface IAuditRecordStorageAdapter : IStorageAdapter<AuditRecord>
    {
    }
}