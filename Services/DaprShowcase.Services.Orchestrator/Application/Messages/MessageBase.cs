using System;
using System.Text;

namespace DaprShowcase.Services.Orchestrator.Application.Messages
{
    public abstract class MessageBase : IMessage
    {
        private static readonly Random _random = new Random();

        protected MessageBase()
        {
            Id = $"{Prefix}-{GetUtcNowToString()}-{GetRandomString(5)}";
        }

        public string Id { get; set; }
        protected abstract string Prefix { get; }

        protected string GetUtcNowToString()
        {
            DateTime date = DateTime.UtcNow;
            return $"{date.Year}{date.Month:00}{date.Day:00}{date.Hour:00}{date.Minute:00}{date.Second:00}{date.Millisecond:000}";
        }
        protected string GetRandomString(int size, bool lowerCase = true)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}