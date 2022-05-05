using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Adapters.BlobStorage
{
    public class UploadBlobResponse
    {
        public string BlobUrl { get; set; }
    }

    public interface IBlobStorageAdapter
    {
        Task<UploadBlobResponse> UploadBlobAsync(Stream blob, string filename, string contentType);
        Task<Stream> DownloadBlobAsync(string filename);
        Task<Stream> DeleteBlobAsync(string filename);
    }
}