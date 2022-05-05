using System.Collections.Specialized;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Messages.Commands;

namespace DaprShowcase.Common.Adapters.OrchestratorPublisher
{
    public interface IOrchestratorPublisherAdapter
    {
        Task PublishRunWorkflowCommandAsync(string workflowId, string companyId, string folderId, string filename, string callbackUrl);
        Task PublishContinueWorkflowCommandAsync<TData>(string workflowId, TData data = null) where TData : class;
        Task PublishStopWorkflowCommandAsync<TData>(string workflowId, TData data = null) where TData : class;
    }
}