using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Domain.Entities;

namespace DaprShowcase.Services.AuditingWorker.Domain.Entities
{
    public class AuditRecord : EntityBase
    {
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Action { get; set; }
        public string DeltaJson { get; set; }
        public string MadeBy { get; set; }
        public System.DateTime MadeOn { get; set; }
    }
}