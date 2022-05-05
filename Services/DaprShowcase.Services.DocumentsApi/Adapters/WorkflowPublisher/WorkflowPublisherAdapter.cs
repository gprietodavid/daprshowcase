using Dapr.Client;
using DaprShowcase.Common.Adapters.WorkflowPublisher;

namespace DaprShowcase.Services.DocumentsApi.Adapters.WorkflowPublisher
{
    public class WorkflowPublisherAdapter : WorkflowCommandPublisherAdapter
    {
        public WorkflowPublisherAdapter(DaprClient dapr) : base(dapr)
        {
        }
    }
}