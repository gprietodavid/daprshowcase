using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using DaprShowcase.Common.Extensions;
using Microsoft.VisualBasic;

namespace DaprShowcase.Common.Adapters.BlobStorage
{
    public abstract class AzureBlobStorageDaprBindingBlobStorageAdapterBase : IBlobStorageAdapter
    {
        // https://docs.dapr.io/reference/components-reference/supported-bindings/blobstorage/
        // https://github.com/dapr/dotnet-sdk/issues/560

        private readonly DaprClient _dapr;

        public abstract string BindingName { get; }

        protected AzureBlobStorageDaprBindingBlobStorageAdapterBase(DaprClient dapr)
        {
            _dapr = dapr;
        }

        public virtual async Task<UploadBlobResponse> UploadBlobAsync(Stream blob, string filename, string contentType)
        {
            var bindingRequest = new BindingRequest(BindingName, "create");
            bindingRequest.Metadata.Add("blobName", filename);
            bindingRequest.Metadata.Add("contentType", contentType);
            bindingRequest.Data = new ReadOnlyMemory<byte>(blob.ConvertToBase64Stream().ToByteArray());

            var response = await _dapr.InvokeBindingAsync(bindingRequest);

            return new UploadBlobResponse();
        }
        public virtual async Task<Stream> DownloadBlobAsync(string filename)
        {
            var bindingRequest = new BindingRequest(BindingName, "get");
            bindingRequest.Metadata.Add("blobName", filename);

            var response = await _dapr.InvokeBindingAsync(bindingRequest);
            var byteArray = response.Data.ToArray();

            return new MemoryStream(byteArray);
        }
        public virtual Task<Stream> DeleteBlobAsync(string filename)
        {
            throw new NotImplementedException();
        }
    }
}