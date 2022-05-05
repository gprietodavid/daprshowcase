using Dapr.Client;
using DaprShowcase.Common.Adapters.BlobStorage;

namespace DaprShowcase.Services.AvScanWorker.Adapters
{
    public class DocumentBlobStorageAdapter : AzureBlobStorageDaprBindingBlobStorageAdapterBase, IDocumentBlobStorageAdapter
    {
        public override string BindingName { get; } = "documentsbinding";

        public DocumentBlobStorageAdapter(DaprClient dapr) : base(dapr)
        {
        }
    }
}