using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaprShowcase.Common.Domain.Entities;

namespace DaprShowcase.Services.DocumentsApi.Domain
{
    public class CompanyFolder : IEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
    }
}