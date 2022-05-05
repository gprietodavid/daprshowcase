namespace DaprShowcase.Common.Application.Messages.Commands
{
    public class DownloadBlobCommand : BlobCommandBase
    {
        public override string Topic { get; } = "daprshowcase-download-file";
    }
}