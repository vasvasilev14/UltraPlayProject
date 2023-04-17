using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UltraPlay.Data.Interfaces;


namespace UltraPlay.Data.Models
{
    public class Odd : IDeletableEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double? SpecialBetValue { get; set; }
        public int BetId { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
