using System;
using DaprShowcase.Common;
using DaprShowcase.Common.Application.Messages.Commands;
using DaprShowcase.Services.ZipWorker.Application.Handlers.Commands;

namespace DaprShowcase.Services.ZipWorker
{
    public class Subscriptions : MessageSubscriptionCollection
    {
        public Subscriptions(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Add<ZipBlobCommand, ZipBlobCommandHandler>("workflowpubsub", "daprshowcase-zip-file");
        }
    }
}