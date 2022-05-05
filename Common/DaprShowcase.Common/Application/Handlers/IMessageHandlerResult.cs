using System.Collections.Generic;

namespace DaprShowcase.Common.Application.Handlers
{
    public interface IMessageHandlerResult
    {
        bool IsValid { get; }
        bool IsNotValid { get; }
        IReadOnlyList<string> Messages { get; }
        object Result { get; }
    }
}