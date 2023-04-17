using System.Xml.Serialization;

namespace UltraPlay.Services.Utils.XMLReader.Models
{
    [XmlType("Odd")]
    public class OddDTO
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public double Value { get; set; }

        [XmlAttribute("SpecialBetValue")]
        public double SpecialBetValue { get; set; }
    }
}
