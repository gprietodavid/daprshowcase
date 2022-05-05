using System;
using System.Threading.Tasks;
using DaprShowcase.Common.Application.Messages;

namespace DaprShowcase.Common.Application.Handlers
{
    public interface IMessageHandler
    {
    }

    public interface IMessageHandler<TMessage> : IMessageHandler
        where TMessage : IMessage
    {
        Task<IMessageHandlerResult> HandleAsync(TMessage msg);
        Task<IMessageHandlerResult> HandleWithRetryAsync(TMessage msg, int retryCount, Func<int,TimeSpan> sleepDurationFunc, Action<Exception> onExceptionAction);
    }
}