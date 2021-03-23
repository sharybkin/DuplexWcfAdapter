using System.Collections.Generic;
using Common.MessageContracts;

namespace BettingLine.Service.BackgroundService
{
    public interface IMessagePublisher
    {
        void Publish<T>(string exchange, T message)
            where T: Message;

    }
}