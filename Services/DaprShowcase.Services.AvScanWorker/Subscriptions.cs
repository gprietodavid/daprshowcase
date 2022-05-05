using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaprShowcase.Common;
using DaprShowcase.Common.Application.Messages.Commands;
using DaprShowcase.Services.AvScanWorker.Application.Handlers.Commands;

namespace DaprShowcase.Services.AvScanWorker
{
    public class Subscriptions : MessageSubscriptionCollection
    {
        public Subscriptions(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Add<ScanBlobCommand, ScanBlobCommandHandler>("workflowpubsub", "daprshowcase-scan-file");
        }
    }
}