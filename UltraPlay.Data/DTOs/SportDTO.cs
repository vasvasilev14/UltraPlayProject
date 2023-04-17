using System.Xml.Serialization;

namespace UltraPlay.Services.Utils.XMLReader.Models
{
    [XmlType("Sport")]
    public class SportDTO
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Event")]
        public EventDTO[] Events { get; set; }
    }
}
