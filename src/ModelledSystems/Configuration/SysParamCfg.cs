using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

public class SysParamCfg
{
    private double from = double.NaN;
    private double to = double.NaN;
    private double value = double.NaN;

    [XmlAttribute("range")]
    public string rangeText;

    [XmlAttribute("value")]
    public string valueText;

    [XmlAttribute("name")]
    public string Name { get; set; }

    public double From
    {
        get
        {
            if (double.IsNaN(from))
            {
                from = Convert.ToDouble(Regex.Split(rangeText, "\\.\\.")[0].Trim());
            }

            return from;
        }
    }

    public double To
    {
        get
        {
            if (double.IsNaN(to))
            {
                to = Convert.ToDouble(Regex.Split(rangeText, "\\.\\.")[1].Trim());
            }

            return to;
        }
    }

    public double Value
    {
        get
        {
            if (double.IsNaN(value))
            {
                value = ConfigUtils.ParseParameterValue(valueText);
            }

            return value;
        }
    }
}
