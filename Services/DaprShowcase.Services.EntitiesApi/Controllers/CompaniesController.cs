using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Adapters;
using DaprShowcase.Common.Adapters.AuditingPublisher;
using DaprShowcase.Services.EntitiesApi.Adapters;
using DaprShowcase.Services.EntitiesApi.Adapters.CompanyStorage;
using DaprShowcase.Services.EntitiesApi.Domain;
using DaprShowcase.Services.EntitiesApi.Models;

namespace DaprShowcase.Services.EntitiesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyStorageAdapter _companyStorage;
        private readonly IAuditingPublisherAdapter _auditingPublisher;

        public CompaniesController(ICompanyStorageAdapter companyStorage, IAuditingPublisherAdapter auditingPublisher)
        {
            _companyStorage = companyStorage;
            _auditingPublisher = auditingPublisher;
        }

        [HttpGet]
        public async Task<IEnumerable<Company>> Get()
        {
            return await _companyStorage.GetAllAsync();
        }
        
        [HttpGet("{companyId}")]
        public async Task<Company> Get(Guid companyId)
        {
            return await _companyStorage.GetAsync(companyId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewCompanyModel model)
        {
            var company = new Company() { Id = Guid.NewGuid(), Name = model.Name, CreatedOn = DateTime.UtcNow };
            var companyId = await _companyStorage.AddAsync(company);
            await _auditingPublisher.PublishDomainEntityCreatedAsync(company, "test");

            return Ok(companyId);
        }

        [HttpPut("{companyId}")]
        public async Task<IActionResult> Put(Guid companyId, [FromBody] UpdateCompanyModel model)
        {
            var company = new Company() { Id = Guid.NewGuid(), Name = model.Name, CreatedOn = DateTime.UtcNow };
            await _companyStorage.UpdateAsync(company);

            return Ok(company.Id.ToString());
        }

        [HttpDelete("{companyId}")]
        public async Task<IActionResult> Delete(Guid companyId)
        {
            await _companyStorage.DeleteAsync(companyId);

            return Ok();
        }
    }
}