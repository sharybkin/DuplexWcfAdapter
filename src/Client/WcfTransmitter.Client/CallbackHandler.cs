using System;
using System.ServiceModel;

namespace WcfTransmitter.Client
{
    public class CallbackHandler: BettingLineService.IBettingLineServiceCallback
    {
        public void Outcomes(BettingLineService.Outcome[] outcomes)
        {
            foreach (var outcome in outcomes)
            {
                Console.WriteLine($"Callback: Received Outcome: Event - [{outcome.EventName}], BetName - [{outcome.BetName}], Outcome - [{outcome.Title}], Factor - {outcome.FactorTime}. CreatedTime: {outcome.FactorTime}");
            }
        }
    }
}
