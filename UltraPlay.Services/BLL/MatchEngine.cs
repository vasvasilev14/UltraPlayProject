using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using UltraPlay.Data.Interfaces;
using UltraPlay.Data.ViewModels;
using UltraPlay.Services.Matches.Interfaces;

namespace UltraPlay.Services.Matches
{
    public class MatchEngine : IMatchEngine
    {
        private readonly ILogger<MatchEngine> _logger;
        private readonly IMatchRepository _matchRepository;

        private static readonly List<string> previewBets = new List<string> { "Match Winner", "Map Advantage", "Total Maps Played" };

        public MatchEngine(ILogger<MatchEngine> logger, IMatchRepository matchRepository)
        {
            _logger = logger;
            _matchRepository = matchRepository;
        }

        public async Task<MatchViewModel> GetMatchByIdAsync(int matchId, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();
            _logger.LogInformation($"Getting match by ID [{matchId}].");
            return await _matchRepository.GetMatchByIdAsync(matchId, cToken);
        }

        public async Task<List<MatchViewModel>> GetMatchesIn24Hours(CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();
            var models = await _matchRepository.GetMatchesIn24Hours(previewBets, cToken);
            var matchIds = models.Select(m => m.ID).ToList();
            _logger.LogInformation($"Getting all matches in 24 hours [{string.Join(',', matchIds)}].");
            return models;
        }
    }
}
