using Microsoft.EntityFrameworkCore;
using UltraPlay.Data.Constants;
using UltraPlay.Data.Enums;
using UltraPlay.Data.Interfaces;
using UltraPlay.Data.Models;
using UltraPlay.Services.Utils.XMLReader.Models;

namespace UltraPlay.Data.Repositories
{
    public class SportRepository : ISportRepository
    {
        private readonly AppDbContext _context;

        public SportRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task UpdataDatabaseAsync(SportDTO[] sportDTOs, CancellationToken cToken)
        {
            var sports = new List<Sport>();
            var events = new List<Event>();
            var matches = new List<Match>();
            var bets = new List<Bet>();
            var odds = new List<Odd>();

            foreach (var sport in sportDTOs)
            {
                var newSport = new Sport
                {
                    ID = sport.ID,
                    Name = sport.Name,
                };

                foreach (var eventDTO in sport.Events)
                {
                    var newEvent = new Event
                    {
                        ID = eventDTO.ID,
                        Name = eventDTO.Name,
                        IsLive = eventDTO.IsLive,
                        CategoryID = eventDTO.CategoryID,
                        SportID = newSport.ID
                    };
                    events.Add(newEvent);

                    foreach (var match in eventDTO.Matches)
                    {
                        Enum.TryParse(match.MatchType, out EventMatchType matchType);

                        var newMatch = new Match
                        {
                            ID = match.ID,
                            Name = match.Name,
                            StartDate = match.StartDate,
                            MatchType = matchType,
                        };

                        foreach (var bet in match.Bets)
                        {
                            var newBet = new Bet
                            {
                                ID = bet.ID,
                                Name = bet.Name,
                                IsLive = bet.IsLive,
                            };
                            foreach (var odd in bet.Odds)
                            {
                                var newOdd = new Odd
                                {
                                    ID = odd.ID,
                                    Name = odd.Name,
                                    Value = odd.Value,
                                    SpecialBetValue = odd.SpecialBetValue == 0 ? null : odd.SpecialBetValue
                                };
                                odds.Add(newOdd);
                                newBet.Odds.Add(newOdd);
                            }
                            bets.Add(newBet);
                            newMatch.Bets.Add(newBet);
                        }
                        matches.Add(newMatch);
                        newEvent.Matches.Add(newMatch);
                    }
                    newSport.Events.Add(newEvent);
                }
                sports.Add(newSport);
            }

            var eventIDs = events.Select(e => e.ID).ToList();
            var matchIDs = matches.Select(m => m.ID).ToList();
            var betIDs = bets.Select(b => b.ID).ToList();
            var oddIDs = odds.Select(o => o.ID).ToList();

            CheckForIsValid(eventIDs, matchIDs, betIDs, oddIDs);
            await CheckForUpdate(matches, odds);


            var dbSportIDs = _context.Sports.Select(e => e.ID).ToList();
            var newSports = sports.Where(e => !dbSportIDs.Contains(e.ID)).ToList();

            if (newSports.Count() > 0)
            {
                _context.Sports.AddRange(newSports);

            }
            else
            {
                var dbEventIDs = _context.Events.Select(e => e.ID).ToList();
                var newEvents = events.Where(e => !dbEventIDs.Contains(e.ID)).ToList();
                _context.Events.AddRange(newEvents);
            }

            await _context.SaveChangesAsync();
        }


        #region Validators

        private async Task CheckForUpdate(List<Match> matches, List<Odd> odds)
        {
            foreach (var match in matches)
            {
                var dbMatch = await _context.Matches.Where(m => m.ID == match.ID && m.DateDeleted == null).FirstOrDefaultAsync();
                if (dbMatch != null)
                {
                    if (dbMatch.MatchType != match.MatchType)
                    {
                        dbMatch.MatchType = match.MatchType;
                        AddMatchLogger(dbMatch.ID, Constans.MatchUpdated);

                    }
                    if (dbMatch.StartDate != match.StartDate)
                    {
                        dbMatch.StartDate = match.StartDate;
                        AddMatchLogger(dbMatch.ID, Constans.MatchUpdated);
                    }
                }
            }

            foreach (var odd in odds)
            {
                var dbOdd = await _context.Odds.Where(o => o.ID == odd.ID && o.DateDeleted == null).FirstOrDefaultAsync();
                if (dbOdd != null && dbOdd.Value != odd.Value)
                {
                    dbOdd.Value = odd.Value;
                    AddOddLogger(dbOdd.ID, Constans.OddUpdated);
                }
            }

        }

        private void CheckForIsValid(List<int> eventsIDs, List<int> matchesIDs, List<int> betsIDs, List<int> oddsIDs)
        {
            var eventsForRemove = _context.Events.Where(e => !eventsIDs.Contains(e.ID) && e.DateDeleted == null).ToList();
            foreach (var eventForRemove in eventsForRemove)
            {
                eventForRemove.DateDeleted = DateTime.UtcNow;
            }

            var matchesForRemove = _context.Matches.Where(m => !matchesIDs.Contains(m.ID) && m.DateDeleted == null).ToList();
            foreach (var matchForRemove in matchesForRemove)
            {
                matchForRemove.DateDeleted = DateTime.UtcNow;
                AddMatchLogger(matchForRemove.ID, Constans.MatchRemoved);
            }

            var betsForRemove = _context.Bets.Where(b => !betsIDs.Contains(b.ID) && b.DateDeleted == null).ToList();
            foreach (var betForRemove in betsForRemove)
            {
                betForRemove.DateDeleted = DateTime.UtcNow;
                var log = new BetLogger()
                {
                    BetID = betForRemove.ID,
                    Message = Constans.BetRemoved
                };
                _context.BetLoggers.Add(log);
            }

            var oddsForRemove = _context.Odds.Where(o => !oddsIDs.Contains(o.ID) && o.DateDeleted == null).ToList();
            foreach (var oddForRemove in oddsForRemove)
            {
                oddForRemove.DateDeleted = DateTime.UtcNow;
                AddOddLogger(oddForRemove.ID, Constans.OddRemoved);
            }
        }
        #endregion

        #region Loggers

        private void AddMatchLogger(int matchID, string message)
        {
            var log = new MatchLogger()
            {
                MatchID = matchID,
                Message = message
            };
            _context.MatchLoggers.Add(log);
        }

        private void AddOddLogger(int oddID, string message)
        {
            var log = new OddLogger()
            {
                OddID = oddID,
                Message = message
            };
            _context.OddLoggers.Add(log);
        }
        #endregion
    }
}
