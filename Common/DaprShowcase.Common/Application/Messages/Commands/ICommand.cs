using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Application.Messages.Commands
{
    public interface ICommand : IMessage
    {
        string Topic { get; }
    }
}