using Roads.Models;
using System.Xml;
using System.Xml.Linq;

namespace Roads.Services
{
    public class FileDataProvider : IDataProvider
    {
        const string fileName = "monaco-latest.osm";
        //const string fileName = "switzerland-latest.osm";
        const string filePath = $"..//..//..//Resources//{fileName}";

        public Dictionary<long, Node> GetNodes()
        {
            var reader = new XmlTextReader($"Resources//{fileName}");
            var nodes = new Dictionary<long, Node>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "node")
                {
                    var id = long.Parse(reader.GetAttribute("id"));
                    var lat = double.Parse(reader.GetAttribute("lat"));
                    var lon = double.Parse(reader.GetAttribute("lon"));
                    nodes.Add(
                        id,
                        new Node
                        {
                            Latitude = lat,
                            Longitude = lon,
                        });
                }
            }

            return nodes;
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
                                Latitude = double.Parse(reader.GetAttribute("lat")),
                                Longitude = double.Parse(reader.GetAttribute("lon")),
                            };
                        }
                    }
                }
            }

            /*while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "node")
                {
                    var id = long.Parse(reader.GetAttribute("id"));
                    var lat = double.Parse(reader.GetAttribute("lat"));
                    var lon = double.Parse(reader.GetAttribute("lon"));
                    nodes.Add(
                        id,
                        new Node
                        {
                            Latitude = lat,
                            Longitude = lon,
                        });
                }
            }

            return nodes;*/
        }

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


        public Dictionary<long, Way> GetWaysDictionary()
        {
            var reader = new XmlTextReader($"Resources//{fileName}");
            var ways = new Dictionary<long, Way>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "way")
                {
                    var id = long.Parse(reader.GetAttribute("id"));

                    var nodeIds = new List<long>();
                    var tags = new Dictionary<string, string>();

                    while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "way"))
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "nd")
                        {
                            nodeIds.Add(long.Parse(reader.GetAttribute("ref")));
                        }

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "tag")
                        {
                            tags.Add(reader.GetAttribute("k"), reader.GetAttribute("v"));
                        }
                    }

                    ways.Add(
                        id,
                        new Way
                        {
                            Id = id,
                            NodeIds = nodeIds.Count > 0 ? nodeIds.ToArray() : null,
                            //Tags = tags.Count > 0 ? tags : null,
                        });
                }
            }

            return ways;
        }

        public Dictionary<long, Way> GetWaysOld()
        {
            var reader = new XmlTextReader($"Resources//{fileName}");
            var ways = new Dictionary<long, Way>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "way")
                {
                    var id = long.Parse(reader.GetAttribute("id"));

                    var nodeIds = new List<long>();
                    var tags = new Dictionary<string, string>();

                    while (reader.Read() && !(reader.NodeType == XmlNodeType.EndElement && reader.Name == "way"))
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "nd")
                        {
                            nodeIds.Add(long.Parse(reader.GetAttribute("ref")));
                        }

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "tag")
                        {
                            tags.Add(reader.GetAttribute("k"), reader.GetAttribute("v"));
                        }
                    }

                    ways.Add(
                        id,
                        new Way
                        {
                            Id = id,
                            NodeIds = nodeIds.Count > 0 ? nodeIds.ToArray() : null,
                            //Tags = tags.Count > 0 ? tags : null,
                        });
                }
            }

            return ways;
        }

        public List<long> GetNodesIds()
        {
            var reader = new XmlTextReader($"Resources//{fileName}");
            var nodes = new List<long>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "node")
                {
                    var id = long.Parse(reader.GetAttribute("id"));
                    nodes.Add(id);
                }
            }

            return nodes;
        }

        public Dictionary<long, Node> GetNodes3()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load($"Resources//{fileName}");

            var nodes = new Dictionary<long, Node>();

            foreach (XmlNode xmlNode in doc.DocumentElement.ChildNodes)
            {
                if (xmlNode.Name == "node")
                {
                    var id = long.Parse(GetXmlAttributeValue(xmlNode, "id"));
                    var lat = double.Parse(GetXmlAttributeValue(xmlNode, "lat"));
                    var lon = double.Parse(GetXmlAttributeValue(xmlNode, "lon"));
                    nodes.Add(
                        id,
                        new Node
                        {
                            Latitude = lat,
                            Longitude = lon,
                        });
                }
            }

            return nodes;
        }

        private static string GetXmlAttributeValue(XmlNode xmlNode, string attributeName)
            => xmlNode.Attributes[attributeName].Value;
    }
}
