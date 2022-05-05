using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DaprShowcase.Common.Adapters.DataStorage;
using DaprShowcase.Services.DocumentsApi.Domain;

namespace DaprShowcase.Services.DocumentsApi.Adapters.CompanyFolderStorage
{
    public interface ICompanyFolderStorageAdapter : IStorageAdapter<CompanyFolder>
    {
        Task<IEnumerable<CompanyFolder>> GetAllAsync(Guid companyId);
    }
}