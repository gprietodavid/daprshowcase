using DaprShowcase.Common;
using Microsoft.Extensions.Logging;

namespace DaprShowcase.Services.ZipWorker
{
    public class WorkerProcessService : WorkerProcessServiceBase
    {
        public WorkerProcessService(ILogger<WorkerProcessService> logger, IMessageSubscriptionCollection subscriptions) : base(logger, subscriptions)
        {
        }
    }
}