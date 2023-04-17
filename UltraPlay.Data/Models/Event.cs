using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraPlay.Data.Interfaces;


namespace UltraPlay.Data.Models
{
    public class Event : IDeletableEntity
    {
        public Event()
        {
            Matches = new HashSet<Match>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsLive { get; set; }
        public int CategoryID { get; set; }
        public int SportID { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
        public DateTime? DateDeleted { get; set; }

    }
}
