using System;
using System.ServiceModel;
using System.Threading.Tasks;


namespace WcfTransmitter.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(2000);

            BettingLineService.BettingLineServiceClient client = null;
            try
            {
                InstanceContext instanceContext = new InstanceContext(new CallbackHandler());

                client = new BettingLineService.BettingLineServiceClient(instanceContext);

                foreach (BettingLineService.Outcome outcome in await client.GetOutcomesAsync())
                    Console.WriteLine($"GetOutcomes: Received Outcome: Event - [{outcome.EventName}], BetName - [{outcome.BetName}], Outcome - [{outcome.Title}], Factor - {outcome.FactorTime}. CreatedTime: {outcome.FactorTime}");

                await client.SubscribeAsync();

                await Task.Delay(30000);

                await client.UnsubscribeAsync();
            }
            catch (TimeoutException timeProblem)
            {
                Console.WriteLine("The service operation timed out. " + timeProblem.Message);
                Console.ReadLine();
                client.Abort();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message);
                Console.ReadLine();
                client.Abort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Abort();
                client.Close();
            }

            Console.ReadLine();
        }
    }
}
