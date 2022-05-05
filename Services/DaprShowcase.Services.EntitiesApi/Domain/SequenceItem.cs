using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaprShowcase.Common.Domain.Entities;

namespace DaprShowcase.Services.EntitiesApi.Domain
{
    public class SequenceItem : IEntity
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
    }
}
