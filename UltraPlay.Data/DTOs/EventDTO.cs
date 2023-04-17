using System.Xml;
using System.Xml.Serialization;

namespace UltraPlay.Services.Utils.XMLReader.Models
{
    [XmlType("Event")]
    public class EventDTO
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("IsLive")]
        public bool IsLive { get; set; }

        [XmlAttribute("CategoryID")]
        public int CategoryID { get; set; }

        [XmlElement("Match")]
        public MatchDTO[] Matches { get; set; }

        //public int SportID { get; set; } = XmlElement.ParentNode.ID;
    }
}
