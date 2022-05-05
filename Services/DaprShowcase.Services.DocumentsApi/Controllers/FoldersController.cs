using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaprShowcase.Services.DocumentsApi.Adapters;
using DaprShowcase.Services.DocumentsApi.Adapters.CompanyFolderStorage;
using DaprShowcase.Services.DocumentsApi.Adapters.EntitiesApi;
using DaprShowcase.Services.DocumentsApi.Domain;
using DaprShowcase.Services.DocumentsApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DaprShowcase.Services.DocumentsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoldersController : ControllerBase
    {
        private readonly IEntitiesApiAdapter _entitiesApi;
        private readonly ICompanyFolderStorageAdapter _companyFolderStorage;

        public FoldersController(IEntitiesApiAdapter entitiesApi, ICompanyFolderStorageAdapter companyFolderStorage)
        {
            _entitiesApi = entitiesApi;
            _companyFolderStorage = companyFolderStorage;
        }

        [HttpGet]
        public async Task<IEnumerable<CompanyFolder>> Get()
        {
            return await _companyFolderStorage.GetAllAsync();
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewCompanyFolderModel model)
        {
            var companyIdIsGuid = Guid.TryParse(model.CompanyId, out Guid companyId);
            if (!companyIdIsGuid) return BadRequest($"Bad company ID {model.CompanyId}!");
            var company = await _entitiesApi.GetCompanyAsync(Guid.Parse(model.CompanyId));
            if (company == null) return BadRequest($"Company with ID {model.CompanyId} does not exist!");

            var companyFolder = new CompanyFolder() { CompanyId = companyId, Id = Guid.NewGuid(), Name = model.Name };
            var companyFolderId = await _companyFolderStorage.AddAsync(companyFolder);

            return Ok(companyFolderId);
        }
    }
}