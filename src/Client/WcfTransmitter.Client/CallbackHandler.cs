using System;
using System.ServiceModel;
using WcfTransmitter.Client.Extensions;

namespace WcfTransmitter.Client
{
    public class CallbackHandler: BettingLineService.IBettingLineServiceCallback
    {
        public void Outcomes(BettingLineService.Outcome[] outcomes)
        {
            foreach (var outcome in outcomes)
            {
                outcome.PrintToConsole("CallbackHandler");
            }
        }
    }
}
