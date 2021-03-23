using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BettingLine.Service.Repository;
using Common.MessageContracts;
using Microsoft.Extensions.Logging;

namespace BettingLine.Service.BackgroundService
{
    public class OutcomesChangesProcessorBackgroundService : IHostedService
    {
        private readonly IOutcomeRepository _outcomeRepository;
        private readonly ILogger<OutcomesChangesProcessorBackgroundService> _logger;
        private readonly IMessagePublisher _outcomesMessagePublisher;

        public OutcomesChangesProcessorBackgroundService(IOutcomeRepository outcomeRepository, ILogger<OutcomesChangesProcessorBackgroundService> logger, IMessagePublisher outcomesMessagePublisher)
        {
            _outcomeRepository = outcomeRepository;
            _logger = logger;
            _outcomesMessagePublisher = outcomesMessagePublisher;
        }

        public  Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run( () => StartPublisher(cancellationToken), cancellationToken);
            _logger.LogInformation($"{nameof(OutcomesChangesProcessorBackgroundService)} started");
            return Task.CompletedTask;
        }

        private async Task StartPublisher(CancellationToken cancellationToken)
        {
            while (true)
            {
                var outcomes = await _outcomeRepository.GetOutcomesChangesAsync();
                OutcomesMessage message = new OutcomesMessage {Outcomes = outcomes};
                _outcomesMessagePublisher.Publish(MessageProperties.Outcomes.Exchange, message);
                 
                try
                {
                    //You can use Quartz.Net instead if you need a schedule.
                    await Task.Delay(3000, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning($"{nameof(OutcomesChangesProcessorBackgroundService)} has been stopped.");
        }
    }
}
