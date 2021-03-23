using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Common.Models;
using WcfTransmitter.Service.WcfEndpoint.Contracts;

namespace WcfTransmitter.Service.WcfEndpoint
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    internal class BettingLineService : IBettingLineService
    {
        private OutcomesChangesReceiver _outcomesChangesReceiver;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        private HttpClient _httpClient;

        private HttpClient HttpClient => GetClient();

        private HttpClient GetClient()
        {
            if (_httpClient == null)
            {
                var handler = new HttpClientHandler();
                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(ConfigurationManager.AppSettings["BettingLineRestServiceUri"])
                };
            }

            return _httpClient;
        }

        public async Task<IEnumerable<Outcome>> GetOutcomes()
        {
            try
            {
                var restService = new BettingLineRestServiceClient(HttpClient);
                return await restService.GetOutcomesAsync();
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetOutcomes failed");
                throw new FaultException("Internal error");
            }
        }

        public Task Subscribe()
        {
            try
            {
                Logger.Info("Subscribe");

                _outcomesChangesReceiver = new OutcomesChangesReceiver(Callback.Outcomes);
                _outcomesChangesReceiver.BeginReceive();
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Subscribe failed");
            }

            return Task.CompletedTask;
        }

        public Task Unsubscribe()
        {
            try
            {
                _outcomesChangesReceiver?.EndReceive();
                _outcomesChangesReceiver = null;
            
                Logger.Info("Unsubscribe");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Unubscribe failed");
            }
            return Task.CompletedTask;
        }

        private static IBettingLineServiceCallback Callback => OperationContext.Current.GetCallbackChannel<IBettingLineServiceCallback>();

    }
}
