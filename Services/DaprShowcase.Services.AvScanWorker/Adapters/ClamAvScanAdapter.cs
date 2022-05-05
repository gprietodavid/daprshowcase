using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using nClam;

namespace DaprShowcase.Services.AvScanWorker.Adapters
{
    public class ClamAvScanResult : IScanResult
    {
        public bool? IsClean { get; }
        public bool IsError { get; }

        public ClamAvScanResult(bool? isClean, bool isError)
        {
            IsClean = isClean;
            IsError = isError;
        }
    }

    public class ClamAvScanAdapter : IScanAdapter
    {
        private readonly ClamClient _clamClient;

        public ClamAvScanAdapter()
        {
            _clamClient = new ClamClient("localhost", 3310);
        }

        public async Task<IScanResult> ScanAsync(Stream blob, string contentType)
        {
            ClamAvScanResult result = null;
            var clamScanResult = await _clamClient.SendAndScanFileAsync(blob);

            switch (clamScanResult.Result)
            {
                case ClamScanResults.Unknown:
                    result = new ClamAvScanResult(null, true);
                    break;
                case ClamScanResults.Clean:
                    result = new ClamAvScanResult(true, false);
                    break;
                case ClamScanResults.VirusDetected:
                    result = new ClamAvScanResult(false, false);
                    break;
                case ClamScanResults.Error:
                    result = new ClamAvScanResult(null, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}