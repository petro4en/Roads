using Roads.Extensions;
using Roads.Models;
using Roads.Services.Conracts;
using System.Xml;

namespace Roads.Services
{
    public class FileDataProvider : IDataProvider
    {
        const string fileName = "monaco-latest.osm";
        //const string fileName = "switzerland-latest.osm";
        const string filePath = $"..//..//..//Resources//{fileName}";

        public IEnumerable<Way> GetWays()
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "way")
                    {
                        var id = long.Parse(reader.GetAttribute("id"));

                        var nodes = new List<long>();
                        var tags = new Dictionary<string, string>();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement)
                                break;

                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "nd":
                                        nodes.Add(long.Parse(reader.GetAttribute("ref")));
                                        break;
                                    case "tag":
                                        tags.Add(reader.GetAttribute("k"), reader.GetAttribute("v"));
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        yield return new Way
                        {
                            Id = id,
                            NodeIds = nodes.ToArray(),
                            //Tags = tags,
                            HighwayType = tags.GetValueOrDefault("highway"),
                            Name = tags.GetValueOrDefault("name"),
                        };
                    }
                }
            }
        }

        public void FillNodes(Dictionary<long, Node?> nodes)
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "node")
                    {
                        var id = long.Parse(reader.GetAttribute("id"));
                        if (nodes.ContainsKey(id))
                        {
                            nodes[id] = new Node
                            {
                                Latitude = reader.GetAttribute("lat").ToDouble(),
                                Longitude = reader.GetAttribute("lon").ToDouble(),
                            };
                        }
                    }
                }
            }
        }
    }
}
