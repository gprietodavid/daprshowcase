using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Adapters.OrchestratorPublisher;
using DaprShowcase.Common.Adapters.WorkflowPublisher;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Handlers.Commands;
using DaprShowcase.Common.Application.Messages.Commands;
using DaprShowcase.Services.AvScanWorker.Adapters;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace DaprShowcase.Services.AvScanWorker.Application.Handlers.Commands
{
    public class ScanBlobCommandHandler : WorkflowCommandHandlerBase<ScanBlobCommand>, IWorkflowCommandHandler<ScanBlobCommand>
    {
        private readonly ILogger<ScanBlobCommandHandler> _logger;
        private readonly IDocumentBlobStorageAdapter _documentBlobStorage;
        private readonly IScanAdapter _scan;
        
        public ScanBlobCommandHandler(TelemetryClient telemetryClient, ILogger<ScanBlobCommandHandler> logger, IDocumentBlobStorageAdapter documentBlobStorage, IOrchestratorPublisherAdapter orchestratorPublisher, IScanAdapter scan) : base(telemetryClient, orchestratorPublisher)
        {
            _logger = logger;
            _documentBlobStorage = documentBlobStorage;
            _scan = scan;
        }
        
        protected override async Task<IMessageHandlerResult> DoHandleAsync(ScanBlobCommand cmd)
        {
            _logger.LogInformation($"Downloading blob [{cmd.FullPath}]");

            var blob = await _documentBlobStorage.DownloadBlobAsync(cmd.FullPath);

            _logger.LogInformation($"Downloaded blob [{cmd.FullPath}] with size [{blob.Length}]");

            var scanResult = await _scan.ScanAsync(blob, "");
            var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(scanResult);

            _logger.LogInformation($"Scanned blob [{cmd.FullPath}] with result [{(scanResult.IsClean == null ? "UNKNOWN" : scanResult.IsClean.Value ? "CLEAN" : "DIRTY")}]");
            
            return scanResult.IsClean == null ? Fail(new string[] { "Unknown" }) : scanResult.IsClean.Value ? Ok(): Fail(new string[] { "Dirty" });
        }
    }
}