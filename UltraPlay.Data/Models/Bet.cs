using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UltraPlay.Data.Interfaces;

namespace UltraPlay.Data.Models
{
    public class Bet : IDeletableEntity
    {
        public Bet()
        {
            Odds = new HashSet<Odd>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsLive { get; set; }
        public int MatchId { get; set; }
        public virtual ICollection<Odd> Odds { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
