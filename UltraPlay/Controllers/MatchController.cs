using Microsoft.AspNetCore.Mvc;
using UltraPlay.Services.Matches.Interfaces;

namespace UltraPlay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchEngine _matchEngine;

        public MatchController(IMatchEngine matchEngine)
        {
            _matchEngine = matchEngine;
        }

        [HttpGet("{matchId:int}")]
        public async Task<ActionResult> GetMatchByIdAsync(int matchId, CancellationToken cToken)
        {
            var match = await _matchEngine.GetMatchByIdAsync(matchId, cToken);
            if (match == null)
            {
                return NotFound(match);
            }
            return Ok(match);
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetMatchesIn24HoursAsync(CancellationToken cToken)
        {
            var matches = await _matchEngine.GetMatchesIn24Hours(cToken);
            return Ok(matches);
        }
    }
}
