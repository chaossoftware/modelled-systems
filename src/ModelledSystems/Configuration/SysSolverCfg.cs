using ChaosSoft.NumericalMethods.Ode;
using System;
using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

public class SysSolverCfg
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("time")]
    public double ModellingTime { get; set; }

    [XmlAttribute("dt")]
    public double Dt { get; set; }

    public SolverType Type =>
        Name.ToLowerInvariant() switch
        {
            "discrete" => SolverType.Discrete,
            "rk4" => SolverType.RK4,
            "rk5" => SolverType.RK5,
            _ => throw new NotSupportedException($"Unknown solver {Name}"),
        };
}
