using System;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace ModelledSystems.Configuration;

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
        InitialConditionsValue.Split(' ').Select(v => ConfigUtils.ParseParameterValue(v)).ToArray();

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
