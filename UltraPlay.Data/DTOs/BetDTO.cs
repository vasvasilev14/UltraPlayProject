using System.Xml.Serialization;

namespace UltraPlay.Services.Utils.XMLReader.Models
{
    [XmlType("Bet")]
    public class BetDTO
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("IsLive")]
        public bool IsLive { get; set; }

        [XmlElement("Odd")]
        public OddDTO[] Odds { get; set; }
    }
}
