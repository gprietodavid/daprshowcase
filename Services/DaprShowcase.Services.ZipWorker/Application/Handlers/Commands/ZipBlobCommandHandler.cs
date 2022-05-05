using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common.Adapters.OrchestratorPublisher;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Handlers.Commands;
using DaprShowcase.Common.Application.Messages.Commands;
using DaprShowcase.Services.ZipWorker.Adapters;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace DaprShowcase.Services.ZipWorker.Application.Handlers.Commands
{
    public class ZipBlobCommandHandler : WorkflowCommandHandlerBase<ZipBlobCommand>, IWorkflowCommandHandler<ZipBlobCommand>
    {
        private readonly ILogger<ZipBlobCommandHandler> _logger;
        private readonly IDocumentBlobStorageAdapter _documentBlobStorage;

        public ZipBlobCommandHandler(TelemetryClient telemetryClient, ILogger<ZipBlobCommandHandler> logger, IDocumentBlobStorageAdapter documentBlobStorage, IOrchestratorPublisherAdapter orchestratorPublisher) : base(telemetryClient, orchestratorPublisher)
        {
            _logger = logger;
            _documentBlobStorage = documentBlobStorage;
        }

        private Stream CreateZipBlob(Stream blob, string filename)
        {
            var zipStream = new MemoryStream();

            blob.Seek(0, SeekOrigin.Begin);

            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                var zipEntry = archive.CreateEntry(filename);

                using (var entryStream = zipEntry.Open())
                {
                    blob.CopyTo(entryStream);
                }
            }

            return zipStream;
        }

        protected override async Task<IMessageHandlerResult> DoHandleAsync(ZipBlobCommand cmd)
        {
            _logger.LogInformation($"Downloading blob [{cmd.FullPath}]");

            using (var blob = await _documentBlobStorage.DownloadBlobAsync(cmd.FullPath))
            {
                _logger.LogInformation($"Downloaded blob [{cmd.FullPath}] with size [{blob.Length}]");

                using (var zipBlob = CreateZipBlob(blob, cmd.FileName))
                {
                    var zipPath = $"{cmd.Path}\\{cmd.FileName}.zip";
                    _logger.LogInformation($"Uploading compressed blob [{zipPath}] with size [{zipBlob.Length}]");

                    zipBlob.Seek(0, SeekOrigin.Begin);
                    await _documentBlobStorage.UploadBlobAsync(zipBlob, $"{zipPath}", "application/zip");

                    _logger.LogInformation($"Uploaded blob [{zipPath}]");
                }
            }

            await Task.CompletedTask;

            return Ok();
        }
    }
}