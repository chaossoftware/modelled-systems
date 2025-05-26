using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

[XmlRoot("Config")]
public class Config
{
    [XmlElement("ModelingTask")]
    public TaskCfg Task { get; set; }

    [XmlElement("Output")]
    public OutputCfg Out { get; set; }

    [XmlArray("Routines")]
    [XmlArrayItem("Routine", typeof(RoutineCfg))]
    public RoutineCfg[] RoutinesList { get; set; }

    [XmlArray("Systems")]
    [XmlArrayItem("System", typeof(SystemCfg))]
    public SystemCfg[] SystemsList { get; set; }

    public RoutineCfg Routine =>
        RoutinesList.First(r => r.Name == Task.Action);

    public SystemCfg System =>
        SystemsList.First(s => s.Name == Task.System);

    public SysSolverCfg Solver =>
        Task.SolverOverride ?? System.SystemSolver;
}
