using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UltraPlay.Data.Enums;

namespace UltraPlay.Services.Utils.XMLReader.Models
{
    [XmlType("Match")]
    public class MatchDTO
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("StartDate")]
        public DateTime StartDate { get; set; }

        [XmlAttribute("MatchType")]
        public string MatchType { get; set; }

        [XmlElement("Bet")]
        public BetDTO[] Bets { get; set; }
    }
}
