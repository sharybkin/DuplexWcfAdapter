using System;
using Common.MessageContracts;

namespace Common.Models
{
    public class Outcome
    {
        public string EventName { get; set; }

        public string Title { get; set; }

        public string BetName { get; set; }

        public float Factor { get; set; }

        public DateTime FactorTime { get; set; }
    }
}
