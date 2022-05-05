using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Services.AuditingWorker.Adapters
{
    public class SequenceAdapter : ISequenceAdapter
    {
        private static object _syncRoot = new object();
        private ConcurrentBag<int> _items = new ConcurrentBag<int>();

        public SequenceAdapter()
        {
        }

        private IEnumerable<int> GetInSequence()
        {
            return (from item in _items orderby item ascending select item).ToArray();
        }
        
        public bool TryAddValue(int value)
        {
            lock (_syncRoot)
            {
                if (value == 1 || (_items.Any() && GetInSequence().Last().Equals(value - 1)))
                {
                    _items.Add(value);
                    return true;
                }
            }

            return false;
        }
        public string GetSequence()
        {
            if (_items == null) return "";
            return string.Join(' ', GetInSequence());
        }
    }
}