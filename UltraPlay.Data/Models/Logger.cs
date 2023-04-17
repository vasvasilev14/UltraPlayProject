using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UltraPlay.Data.Models
{
    public class Logger
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime CreatedOn => DateTime.UtcNow;
        public string Message { get; set; }
    }
}
