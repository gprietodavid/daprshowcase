using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaprShowcase.Common.Adapters;
using DaprShowcase.Common.Adapters.AuditingPublisher;
using DaprShowcase.Services.EntitiesApi.Adapters;
using DaprShowcase.Services.EntitiesApi.Adapters.SequenceStorage;
using DaprShowcase.Services.EntitiesApi.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DaprShowcase.Services.EntitiesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SequenceController : ControllerBase
    {
        private readonly ISequenceStorageAdapter _sequenceStorage;
        private readonly IAuditingPublisherAdapter _auditingPublisher;

        public SequenceController(ISequenceStorageAdapter sequenceStorage, IAuditingPublisherAdapter auditingPublisher)
        {
            _sequenceStorage = sequenceStorage;
            _auditingPublisher = auditingPublisher;
        }

        [HttpGet]
        public IEnumerable<SequenceItem> Get()
        {
            return _sequenceStorage.GetAllAsync().Result;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] int value)
        {
            var item = new SequenceItem { Id = Guid.NewGuid(), Value = value };

            await _sequenceStorage.AddAsync(new SequenceItem { Id = Guid.NewGuid(), Value = value });
            await _auditingPublisher.PublishSequenceItemCreatedAsync(value, "test");
            
            return Ok(item.Id);
        }

        [HttpPost]
        [Route("stress/{value}")]
        public async Task<IActionResult> PostStress([FromRoute] int value)
        {
            await _auditingPublisher.PublishSequenceItemCreatedAsync(1, "test");
            await _auditingPublisher.PublishSequenceItemCreatedAsync(2, "test");
            await _auditingPublisher.PublishSequenceItemCreatedAsync(5, "test");
            await _auditingPublisher.PublishSequenceItemCreatedAsync(4, "test");
            await _auditingPublisher.PublishSequenceItemCreatedAsync(3, "test");

            //for (int i = 1; i <= value; i++)
            //{
            //    await _auditingPublisher.PublishSequenceItemCreatedAsync(i, "test");
            //}

            return Ok();
        }
    }
}