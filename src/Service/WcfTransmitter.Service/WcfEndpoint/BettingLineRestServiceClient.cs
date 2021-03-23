using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common.MessageContracts;
using Common.Models;
using Newtonsoft.Json;

namespace WcfTransmitter.Service.WcfEndpoint
{
    public class BettingLineRestServiceClient
    {
        private readonly HttpClient _httpClient;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public BettingLineRestServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Outcome>> GetOutcomesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Outcomes");
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new Exception(HttpStatusCode.InternalServerError.ToString());
                    }
                    else
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Outcome>>(content);
            }
            catch (Exception e)
            {
                Logger.Error(e, "BettingLineRestService.GetOutcomesAsync error");
                throw;
            }
        }
    }
}