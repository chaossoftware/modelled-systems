using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

public class RoutineCfg
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAnyAttribute]
    public XmlAttribute[] XAttributes { get; set; }

    public string GetString(string name) =>
        XAttributes.First(a => a.Name.Equals(name)).Value;

    public int GetInt(string name) =>
        Convert.ToInt32(GetString(name), CultureInfo.InvariantCulture);

    public double GetDouble(string name) =>
        Convert.ToDouble(GetString(name), CultureInfo.InvariantCulture);
}
