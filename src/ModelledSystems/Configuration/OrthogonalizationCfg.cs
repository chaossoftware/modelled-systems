using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

public class OrthogonalizationCfg
{
    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("interval")]
    public int Interval { get; set; }
}
