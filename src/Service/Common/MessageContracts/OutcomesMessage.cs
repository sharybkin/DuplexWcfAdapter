using System.Collections.Generic;
using Common.Models;

namespace Common.MessageContracts
{
    public class OutcomesMessage : Message
    {
        public IEnumerable<Outcome> Outcomes { get; set; }
    }
}