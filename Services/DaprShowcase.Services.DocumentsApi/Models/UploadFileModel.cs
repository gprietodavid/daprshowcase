using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DaprShowcase.Services.DocumentsApi.Models
{
    public class UploadFileModel
    {
        public string CompanyId { get; set; }
        public string FolderId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string CallbackUrl { get; set; }
        public IFormFile File { get; set; }
    }
}