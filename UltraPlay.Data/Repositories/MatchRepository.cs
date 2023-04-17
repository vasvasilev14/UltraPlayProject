using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UltraPlay.Data.Interfaces;
using UltraPlay.Data.Models;
using UltraPlay.Data.ViewModels;

namespace UltraPlay.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MatchRepository> _logger;

        public MatchRepository(AppDbContext appDbContext, ILogger<MatchRepository> logger)
        {
            _context = appDbContext;
            _logger = logger;
        }

        public async Task<MatchViewModel> GetMatchByIdAsync(int matchId, CancellationToken cToken)
        {
            var model = await _context.Matches
                 .Where(x => x.ID == matchId && x.DateDeleted == null)
                 .Select(x => new MatchViewModel
                 {
                     ID = x.ID,
                     Name = x.Name,
                     StartDate = x.StartDate,
                     ActiveBets = x.Bets.Where(b => b.DateDeleted == null).Select(b => new BetViewModel
                     {
                         ID = b.ID,
                         Name = b.Name,
                         Odds = b.Odds.Where(o => o.DateDeleted == null).Select(o => new OddViewModel
                         {
                             ID = o.ID,
                             Name = o.Name,
                             SpecialBetValue = o.SpecialBetValue,
                             Value = o.Value
                         }).ToList(),
                     }).ToList(),
                     InactiveBets = x.Bets.Where(b => b.DateDeleted != null).Select(b => new BetViewModel
                     {
                         ID = b.ID,
                         Name = b.Name,
                         Odds = b.Odds.Select(o => new OddViewModel
                         {
                             ID = o.ID,
                             Name = o.Name,
                             SpecialBetValue = o.SpecialBetValue,
                             Value = o.Value
                         }).ToList(),
                     }).ToList()
                 })
                 .FirstOrDefaultAsync(cToken);

            _logger.LogInformation($"Getting match [{matchId}] from the repository.");
            return model;
        }

        public async Task<List<MatchViewModel>> GetMatchesIn24Hours(List<string> allowedBets, CancellationToken cToken)
        {
            var models = await _context.Matches
                .Where(x => x.StartDate <= DateTime.Now.AddHours(24) && x.DateDeleted == null)
                .Select(x => new MatchViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    ActiveBets = x.Bets.Where(b => b.DateDeleted == null && allowedBets.Contains(b.Name))
                    .Select(b => new BetViewModel
                    {
                        ID = b.ID,
                        Name = b.Name,
                        Odds = new List<OddViewModel>()
                    }).ToList(),
                })
                .ToListAsync(cToken);

            var bets = models.SelectMany(m => m.ActiveBets).ToList();

            foreach (var bet in bets)
            {
                var odds = _context.Odds.Where(o => o.BetId == bet.ID && o.DateDeleted == null).Select(o => new OddViewModel
                {
                    ID = o.ID,
                    Name = o.Name,
                    SpecialBetValue = o.SpecialBetValue,
                    Value = o.Value,
                }).ToList();

                if (odds.All(o => o.SpecialBetValue != null))
                {
                    var groupOdd = odds.GroupBy(o => o.SpecialBetValue)
                         .Select(o => new OddViewModel
                         {
                             SpecialBetValue = o.Key,
                             ID = o.Select(x => x.ID).FirstOrDefault(),
                             Value = o.Select(x => x.Value).FirstOrDefault(),
                             Name = o.Select(x => x.Name).FirstOrDefault(),
                         })
                         .FirstOrDefault();

                    bet.Odds.Add(groupOdd);
                }
                else
                {
                    bet.Odds.AddRange(odds);
                }
            }

            var matchIds = models.Select(m => m.ID).ToList();
            _logger.LogInformation($"Getting matches [{string.Join(',', matchIds)}] in 24 hours from the repository.");

            return models;
        }
    }
}
