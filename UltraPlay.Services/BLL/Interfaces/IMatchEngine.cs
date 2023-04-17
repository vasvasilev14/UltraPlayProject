using UltraPlay.Data.ViewModels;

namespace UltraPlay.Services.Matches.Interfaces
{
    public interface IMatchEngine
    {
        public Task<MatchViewModel> GetMatchByIdAsync(int matchId, CancellationToken cToken);

        public Task<List<MatchViewModel>> GetMatchesIn24Hours(CancellationToken cToken);
    }
}
