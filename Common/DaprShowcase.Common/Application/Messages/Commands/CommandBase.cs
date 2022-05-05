using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Application.Messages.Commands
{
    public abstract class CommandBase : MessageBase, ICommand
    {
        protected override string Prefix { get; } = "cmd";
        public abstract string Topic { get; }
    }
}