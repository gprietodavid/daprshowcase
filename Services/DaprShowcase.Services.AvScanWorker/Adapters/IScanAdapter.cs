using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Services.AvScanWorker.Adapters
{
    public interface IScanResult
    {
        bool? IsClean { get; }
        bool IsError { get; }
    }

    public interface IScanAdapter
    {
        Task<IScanResult> ScanAsync(Stream blob, string contentType);
    }
}
