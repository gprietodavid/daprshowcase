using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DaprShowcase.Common.Adapters.BlobStorage;
using DaprShowcase.Common.Adapters.OrchestratorPublisher;
using DaprShowcase.Common.Adapters.WorkflowPublisher;
using DaprShowcase.Common.Application.Messages.Commands;
using DaprShowcase.Common.Extensions;
using DaprShowcase.Services.DocumentsApi.Adapters;
using DaprShowcase.Services.DocumentsApi.Adapters.CompanyFolderStorage;
using DaprShowcase.Services.DocumentsApi.Adapters.DocumentBlobStorage;
using DaprShowcase.Services.DocumentsApi.Adapters.EntitiesApi;
using DaprShowcase.Services.DocumentsApi.Models;

namespace DaprShowcase.Services.DocumentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IEntitiesApiAdapter _entitiesApi;
        private readonly ICompanyFolderStorageAdapter _companyFolderStorage;

        public FilesController(IEntitiesApiAdapter entitiesApi, ICompanyFolderStorageAdapter companyFolderStorage)
        {
            _entitiesApi = entitiesApi;
            _companyFolderStorage = companyFolderStorage;
        }

        [HttpGet("{companyId}/{folderId}/{filename}")]
        public async Task<IActionResult> Get(string companyId, string folderId, string filename, [FromServices] IDocumentBlobStorageAdapter blobStorage)
        {
            var command = new DownloadBlobCommand() { CompanyId = companyId, FolderId = folderId, FileName = filename };

            var blobStorageResponse = await blobStorage.DownloadBlobAsync(command.FullPath);
            
            return new FileContentResult(blobStorageResponse.ToByteArray(), "application/pdf")
            {
                FileDownloadName = filename
            };
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromForm] UploadFileModel upload, [FromServices] IDocumentBlobStorageAdapter blobStorage, [FromServices] IWorkflowPublisherAdapter workflowPublisher, [FromServices] IOrchestratorPublisherAdapter orchestratorPublisher)
        {
            var companyIdIsGuid = Guid.TryParse(upload.CompanyId, out Guid companyId);
            if (!companyIdIsGuid) return BadRequest($"Bad company ID {upload.CompanyId}!");
            var company = await _entitiesApi.GetCompanyAsync(companyId);
            if (company == null) return BadRequest($"Company with ID {upload.CompanyId} does not exist!");

            var folderIdIsGuid = Guid.TryParse(upload.FolderId, out Guid folderId);
            if (!folderIdIsGuid) return BadRequest($"Bad folder ID {upload.FolderId}!");
            var folder = await _companyFolderStorage.GetAsync(folderId);
            if (folder == null) return BadRequest($"Folder with ID {upload.FolderId} does not exist!");

            var command = new SaveBlobCommand() { CompanyId = upload.CompanyId, FolderId = upload.FolderId, FileName = upload.FileName, ContentType = upload.ContentType};

            var blobStorageResponse = await blobStorage.UploadBlobAsync(upload.File.OpenReadStream(), command.FullPath, upload.ContentType);
            await orchestratorPublisher.PublishRunWorkflowCommandAsync("", command.CompanyId, command.FolderId, command.FileName, upload.CallbackUrl);
            return Ok(blobStorageResponse.BlobUrl);
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}