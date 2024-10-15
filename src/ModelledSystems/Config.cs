using ChaosSoft.NumericalMethods.Ode;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace ModelledSystems;

[XmlRoot("Config")]
public class Config
{
    [XmlElement("ModelingTask")]
    public TaskCfg Task { get; set; }

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
}

public class TaskCfg
{
    [XmlElement("Action")]
    public string Action { get; set; }

    [XmlElement("System")]
    public string System { get; set; }

    [XmlElement("Orthogonalization")]
    public OrthogonalizationCfg Orthogonalization { get; set; }

    [XmlElement("Output")]
    public OutputCfg Out { get; set; }
}

public class OutputCfg
{
    [XmlAttribute("picWidth")]
    public int PicWidth { get; set; }

    [XmlAttribute("picHeight")]
    public int PicHeight { get; set; }

    [XmlAttribute("picScale")]
    public int PicScale { get; set; }

    [XmlAttribute("outDir")]
    public string Dir { get; set; }

    [XmlAttribute("binaryOutput")]
    public bool BinOutput { get; set; }
}

public class OrthogonalizationCfg
{
    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("interval")]
    public int Interval { get; set; }
}

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

public class SystemCfg
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("Solver")]
    public SysSolverCfg Solver { get; set; }

    [XmlArray("Parameters")]
    [XmlArrayItem("Param", typeof(SysParamCfg))]
    public SysParamCfg[] Params { get; set; }

    [XmlElement("InitialConditions")]
    public string InitialConditionsValue { get; set; }

    [XmlElement("LinearInitialConditions")]
    public string LinearInitialConditionsValue { get; set; }

    public double[] InitialConditions => 
        InitialConditionsValue.Split(' ').Select(v => Convert.ToDouble(v, CultureInfo.InvariantCulture)).ToArray();

    public double[,] LinearInitialConditions
    {
        get
        {
            string[] rows = LinearInitialConditionsValue.Split(';');
            double[,] conditions = new double[rows.Length, rows.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Trim().Split(' ');
                
                for (int j = 0; j < rows.Length; j++)
                {
                    conditions[i, j] = Convert.ToDouble(columns[j].Trim(), CultureInfo.InvariantCulture);
                }
            }

            return conditions;
        }
    }

    public double[] ParamsValues => Params.Select(p => p.Value).ToArray();
}

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
                value = ParseParameterValue(valueText);
            }

            return value;
        }
    }

    private static double ParseParameterValue(string value)
    {
        string[] _operations = new string[] { "/", "*" };
        string operation = _operations.FirstOrDefault(o => value.Contains(o));

        if (operation == null)
        {
            return Convert.ToDouble(value);
        }

        string[] pair = value.Split(operation[0]);

        double val1 = ParseValue(pair[0]);
        double val2 = ParseValue(pair[1]);

        return GetOperationResult(val1, val2, operation);
    }

    private static double ParseValue(string value)
    {
        value = value.Trim();

        return value.Equals("pi", StringComparison.InvariantCultureIgnoreCase) ?
            Math.PI :
            Convert.ToDouble(value);
    }

    private static double GetOperationResult(double val1, double val2, string operation)
    {
        return operation switch
        {
            "*" => val1 * val2,
            "/" => val1 / val2,
            _ => throw new NotImplementedException($"operation {operation} is not recognized"),
        };
    }
}