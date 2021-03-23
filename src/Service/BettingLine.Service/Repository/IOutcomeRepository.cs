using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;

namespace BettingLine.Service.Repository
{
    public interface IOutcomeRepository
    {
        Task<IEnumerable<Outcome>> GetOutcomesAsync();

        Task<IEnumerable<Outcome>> GetOutcomesChangesAsync();
    }
}
