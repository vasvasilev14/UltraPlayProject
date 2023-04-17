using UltraPlay.Data.Models;
using UltraPlay.Services.Utils.XMLReader.Models;

namespace UltraPlay.Services.Utils.XMLReader.Interfaces
{
    public interface IXMLReader
    {
        SportDTO[] ParseSportsDataXML(string sportsData);

        XMLSportsViewModel GetSportsDataFromXML(string sportsData);
    }
}
