
namespace UltraPlay.Data.Models
{
    public class OddLogger : Logger
    {
        public int OddID { get; set; }

        public virtual Odd Odd { get; set; }
    }
}
