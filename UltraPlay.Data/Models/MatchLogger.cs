
namespace UltraPlay.Data.Models
{
    public class MatchLogger : Logger
    {
        public int MatchID { get; set; }

        public virtual Match Match { get; set; }
    }
}
