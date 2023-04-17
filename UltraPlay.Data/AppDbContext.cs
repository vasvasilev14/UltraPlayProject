using Microsoft.EntityFrameworkCore;
using UltraPlay.Data.Models;

namespace UltraPlay.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }


        public DbSet<Bet> Bets { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Odd> Odds { get; set; }

        public DbSet<Sport> Sports { get; set; }

        public DbSet<OddLogger> OddLoggers { get; set; }

        public DbSet<BetLogger> BetLoggers { get; set; }

        public DbSet<MatchLogger> MatchLoggers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=UltraPLayDb;Integrated Security=true;Trusted_Connection=True;TrustServerCertificate=true");
            }
        }
    }
}