using Microsoft.Extensions.Logging;
using System;
using System.Xml;
using System.Xml.Serialization;
using UltraPlay.Data.Enums;
using UltraPlay.Data.Models;
using UltraPlay.Services.Utils.XMLReader.Constants;
using UltraPlay.Services.Utils.XMLReader.Interfaces;
using UltraPlay.Services.Utils.XMLReader.Models;

namespace UltraPlay.Services.Utils.XMLReader
{
    public class XMLReader : IXMLReader
    {
        private readonly ILogger _logger;
        private const string SportsDataRootElement = "XmlSports";

        public XMLReader(ILogger<XMLReader> logger)
        {
            _logger = logger;
        }

        public SportDTO[] ParseSportsDataXML(string sportsData)
        {
            if (string.IsNullOrEmpty(sportsData))
            {
                throw new Exception("Empty string provided as xml data.");
            }

            var reader = new StringReader(sportsData);
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = SportsDataRootElement;
            root.IsNullable = true;
            var serializer = new XmlSerializer(typeof(SportDTO[]), root);

            var parsedData = (SportDTO[])serializer.Deserialize(reader);

            if (parsedData == null)
            {
                throw new Exception("Error while parsing the xml data.");
            }

            _logger.LogInformation("Xml file was parsed successfully.");


            return parsedData;
        }

        /// <summary>
        /// NOT WORKING. ONLY FOR QUANTIONS.
        /// This was my first try to read the xml file. I didnt know how to take the parentID, so i move on and just deserialize the xml.
        /// </summary>
        /// <param name="sportsData"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        #region XMLReader
        public XMLSportsViewModel GetSportsDataFromXML(string sportsData)
        {
            if (string.IsNullOrEmpty(sportsData))
            {
                throw new Exception("Error: The provided xml is empty.");
            }

            var sports = new List<Sport>();
            var events = new List<Event>();
            var matches = new List<Match>();
            var bets = new List<Bet>();
            var odds = new List<Odd>();

            using (var stringReader = new StringReader(sportsData))
            {
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            int parentId;
                            int.TryParse(reader.GetAttribute("ID"), out parentId);

                            do
                            {
                                reader.Read();
                            } while (reader.NodeType != XmlNodeType.Element);

                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case SportsDataNodeNames.Sport:
                                        sports.Add(GetSportDataFromXml(reader));
                                        break;
                                    case SportsDataNodeNames.Event:
                                        events.Add(GetEventDataFromXml(reader, parentId));
                                        break;
                                    case SportsDataNodeNames.Match:
                                        matches.Add(GetMatchDataFromXml(reader, parentId));
                                        break;
                                    case SportsDataNodeNames.Bet:
                                        bets.Add(GetBetDataFromXml(reader, parentId));
                                        break;
                                    case SportsDataNodeNames.Odd:
                                        odds.Add(GetOddDataFromXml(reader, parentId));
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return new XMLSportsViewModel
            {
                Sports = sports,
                Events = events,
                Matches = matches,
                Bets = bets,
                Odds = odds
            };
        }

        private Sport GetSportDataFromXml(XmlReader element)
        {
            return new Sport
            {
                ID = int.Parse(element.GetAttribute(SportNodeAttributes.Id)),
                Name = element.GetAttribute(SportNodeAttributes.Name) ?? string.Empty,
            };
        }

        private Event GetEventDataFromXml(XmlReader element, int parentId)
        {
            return new Event
            {
                ID = int.Parse(element.GetAttribute(EventNodeAttributes.Id)),
                Name = element.GetAttribute(EventNodeAttributes.Name) ?? string.Empty,
                IsLive = bool.Parse(element.GetAttribute(EventNodeAttributes.IsLive)),
                CategoryID = int.Parse(element.GetAttribute(EventNodeAttributes.CategoryId)),
                SportID = parentId
            };
        }

        private Match GetMatchDataFromXml(XmlReader element, int parentId)
        {
            return new Match
            {
                ID = int.Parse(element.GetAttribute(MatchNodeAttributes.Id)),
                Name = element.GetAttribute(MatchNodeAttributes.Name) ?? string.Empty,
                StartDate = DateTime.Parse(element.GetAttribute(MatchNodeAttributes.StartDate)),
                MatchType = Enum.Parse<EventMatchType>(element.GetAttribute(MatchNodeAttributes.MatchType)),
                EventId = parentId
            };
        }

        private Bet GetBetDataFromXml(XmlReader element, int parentId)
        {
            return new Bet
            {
                ID = int.Parse(element.GetAttribute(BetNodeAttributes.Id)),
                Name = element.GetAttribute(BetNodeAttributes.Name) ?? string.Empty,
                IsLive = bool.Parse(element.GetAttribute(BetNodeAttributes.IsLive)),
                MatchId = parentId
            };
        }

        private Odd GetOddDataFromXml(XmlReader element, int parentId)
        {
            var specialBetValue = element.GetAttribute(OddNodeAttributes.SpecialBetValue);

            return new Odd
            {
                ID = int.Parse(element.GetAttribute(OddNodeAttributes.Id)),
                Name = element.GetAttribute(OddNodeAttributes.Name) ?? string.Empty,
                Value = double.Parse(element.GetAttribute(OddNodeAttributes.Value)),
                SpecialBetValue = specialBetValue != null ? double.Parse(specialBetValue) : null,
                BetId = parentId
            };
        }
        #endregion

    }
}