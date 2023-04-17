using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UltraPlay.Data.Enums;
using UltraPlay.Data.Interfaces;


namespace UltraPlay.Data.Models
{
    public class Match : IDeletableEntity
    {
        public Match()
        {
            Bets = new HashSet<Bet>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public EventMatchType MatchType { get; set; }
        public int EventId { get; set; }
        public virtual ICollection<Bet> Bets { get; set; }
        public DateTime? DateDeleted { get; set; }

    }
}
