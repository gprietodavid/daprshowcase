using Dapr.Client;
using DaprShowcase.Common.Adapters.WorkflowPublisher;

namespace DaprShowcase.Services.AvScanWorker.Adapters
{
    public class WorkflowPublisherAdapter : WorkflowCommandPublisherAdapter
    {
        public WorkflowPublisherAdapter(DaprClient dapr) : base(dapr)
        {
        }
    }
}