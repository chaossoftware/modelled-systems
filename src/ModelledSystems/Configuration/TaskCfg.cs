using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

public class TaskCfg
{
    [XmlElement("Action")]
    public string Action { get; set; }

    [XmlElement("System")]
    public string System { get; set; }

    [XmlElement("Orthogonalization")]
    public OrthogonalizationCfg Orthogonalization { get; set; }
}
