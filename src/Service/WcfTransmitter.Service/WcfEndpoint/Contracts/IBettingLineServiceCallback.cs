using System.Collections.Generic;
using System.ServiceModel;
using Common.Models;

namespace WcfTransmitter.Service.WcfEndpoint.Contracts
{
    public interface IBettingLineServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void Outcomes(IEnumerable<Outcome> outcomes);
    }
}
