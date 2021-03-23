using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Common.Models;

namespace WcfTransmitter.Service.WcfEndpoint.Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IBettingLineServiceCallback))]
    public interface IBettingLineService
    {
        [OperationContract]
        Task<IEnumerable<Outcome>> GetOutcomes();

        [OperationContract(Action = "Subscribe", IsOneWay = true)]
        Task Subscribe();

        [OperationContract(Action = "Unsubscribe", IsOneWay = true)]
        Task Unsubscribe();
    }
}
