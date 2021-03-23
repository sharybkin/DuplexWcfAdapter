using System;

namespace Common.MessageContracts
{
    public abstract class Message
    {
        public Guid Id { get; }
        
        public DateTimeOffset DateTimeOffset { get; }
        
        protected Message()
        {
            Id = Guid.NewGuid();
            DateTimeOffset = DateTimeOffset.Now;
        }
    }
}
