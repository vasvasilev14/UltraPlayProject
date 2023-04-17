using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraPlay.Data.Interfaces;

namespace UltraPlay.Data.Models
{
    public class Sport : IDeletableEntity
    {
        public Sport()
        {
            this.Events = new HashSet<Event>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Event> Events { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
