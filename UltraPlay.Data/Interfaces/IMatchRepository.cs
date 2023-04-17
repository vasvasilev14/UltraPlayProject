using UltraPlay.Data.Models;
using UltraPlay.Data.ViewModels;

namespace UltraPlay.Data.Interfaces
{
    public interface IMatchRepository
    {
        public Task<MatchViewModel> GetMatchByIdAsync(int matchId, CancellationToken cToken);

        public Task<List<MatchViewModel>> GetMatchesIn24Hours(List<string> allowedBets, CancellationToken cToken);
    }
}
