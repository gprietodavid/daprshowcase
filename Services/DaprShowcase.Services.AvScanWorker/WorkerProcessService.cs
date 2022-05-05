using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using DaprShowcase.Common;
using DaprShowcase.Common.Adapters.WorkflowPublisher;
using DaprShowcase.Common.Application.Handlers;
using DaprShowcase.Common.Application.Handlers.Commands;
using DaprShowcase.Common.Application.Handlers.Events;
using DaprShowcase.Common.Application.Messages.Commands;
using DaprShowcase.Common.Application.Messages.Events;
using DaprShowcase.Services.AvScanWorker.Application.Handlers.Commands;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DaprShowcase.Services.AvScanWorker
{
    public class WorkerProcessService : WorkerProcessServiceBase
    {
        public WorkerProcessService(ILogger<WorkerProcessService> logger, IMessageSubscriptionCollection subscriptions) : base(logger, subscriptions)
        {
        }
    }
}