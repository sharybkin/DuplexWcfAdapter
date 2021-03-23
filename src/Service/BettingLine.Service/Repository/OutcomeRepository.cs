using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace BettingLine.Service.Repository
{
    public class OutcomeRepository : IOutcomeRepository
    {

        IList<Outcome> _outcomes;

        public OutcomeRepository()
        {
            GenerateOutcomes();
        }

        private void GenerateOutcomes()
        {
            _outcomes = new List<Outcome>
            {
                new Outcome {EventName = "MU vs Chelsea", BetName = "MU win", Title = "true", Factor = GetNewFactor(), FactorTime = DateTime.Now},
                new Outcome {EventName = "MU vs Chelsea", BetName = "MU win", Title = "false", Factor = GetNewFactor(), FactorTime = DateTime.Now}
            };
        }

        private void ChangeOutcomes()
        {
            foreach (var outcome in _outcomes)
            {
                outcome.FactorTime = DateTime.Now;
                outcome.Factor = GetNewFactor();
            }
        }

        private static float GetNewFactor()
        {
            var random = new Random();
            return (float)random.Next(10, 21) / 10f;

        }

        public Task<IEnumerable<Outcome>> GetOutcomesAsync()
        {
            IEnumerable<Outcome> outcomes = _outcomes;
            return Task.FromResult(outcomes);
        }

        public Task<IEnumerable<Outcome>> GetOutcomesChangesAsync()
        {
            ChangeOutcomes();
            
            IEnumerable<Outcome> outcomes = _outcomes;
            return Task.FromResult(outcomes);
        }
    }
}
