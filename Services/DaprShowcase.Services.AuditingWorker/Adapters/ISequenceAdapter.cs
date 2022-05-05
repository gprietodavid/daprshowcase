using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Services.AuditingWorker.Adapters
{
    public interface ISequenceAdapter
    {
        bool TryAddValue(int value);
        string GetSequence();
    }
}