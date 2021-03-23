using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfTransmitter.Client.Extensions
{
    static class OutcomeExtenstion
    {
        public static void PrintToConsole(this BettingLineService.Outcome outcome, string source)
        {
            Console.WriteLine(new String('=',15));
            Console.WriteLine($"Received from [{source}]");
            Console.WriteLine($"Factor time: {outcome.FactorTime}");
            Console.WriteLine($"Event - [{outcome.EventName}], BetName - [{outcome.BetName}]");
            Console.WriteLine($"Outcome - [{outcome.Title}], Factor - {outcome.Factor}.");
            Console.WriteLine(new String('=', 15));
        }
    }
}
