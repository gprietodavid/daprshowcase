using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Application.Messages.Commands
{
    public abstract class BlobCommandBase : CommandBase
    {
        public string CompanyId { get; set; }
        public string FolderId { get; set; }
        public string FileName { get; set; }
        public string Path => $"{CompanyId}\\{FolderId}";
        public string FullPath => $"{Path}\\{FileName}";
    }
}
