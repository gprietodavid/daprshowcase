namespace DaprShowcase.Common.Application.Messages.Commands
{
    public class SaveBlobCommand : BlobCommandBase
    {
        public override string Topic { get; } = "daprshowcase-save-file";
        public string ContentType { get; set; }
    }
}