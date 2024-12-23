using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

public class ChartsCfg
{
    [XmlAttribute("width")]
    public int Width { get; set; }

    [XmlAttribute("height")]
    public int Height { get; set; }

    [XmlAttribute("scale")]
    public double Scale { get; set; }

    [XmlAttribute("labelSize")]
    public float LabelSize { get; set; }

    [XmlAttribute("tickSize")]
    public float TickSize { get; set; }

    [XmlAttribute("padding")]
    public float Padding { get; set; }

    [XmlAttribute("grid")]
    public bool Grid { get; set; }
}
