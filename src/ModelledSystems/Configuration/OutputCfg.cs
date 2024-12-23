using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

public class OutputCfg
{
    [XmlElement("Charts")]
    public ChartsCfg Charts { get; set; }

    [XmlAttribute("outDir")]
    public string Dir { get; set; }

    [XmlAttribute("binaryOutput")]
    public bool BinOutput { get; set; }
}
