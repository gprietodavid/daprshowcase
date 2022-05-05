using System.Collections.Generic;
using System.Linq;

namespace DaprShowcase.Common.Application.Handlers
{
    public class MessageHandlerResult : IMessageHandlerResult
    {
        public bool IsValid { get; }
        public bool IsNotValid => !IsValid;
        public IReadOnlyList<string> Messages { get; }
        public object Result { get; }

        public MessageHandlerResult(bool isValid, params string[] messages)
        {
            IsValid = isValid;
            Messages = messages?.ToList().AsReadOnly() ?? new List<string>().AsReadOnly();
        }
        public MessageHandlerResult(bool isValid, object result, params string[] messages) : this(isValid, messages)
        {
            Result = result;
        }
    }
}