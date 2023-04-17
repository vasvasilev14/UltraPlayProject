
namespace UltraPlay.Data.Models
{
    public class BetLogger : Logger
    {
        public int BetID { get; set; }

        public virtual Bet Bet { get; set; }
    }
}
