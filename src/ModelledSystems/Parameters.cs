using System;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ModelledSystems
{
    internal class Parameters
    {
        private const string ConfigFile = "systems_config.xml";
        private readonly string[] _operations = new string[] { "/", "*" };

        public Parameters()
        {
            try
            {
                var config = XDocument.Load(ConfigFile);
                var modelingTaskEl = config.Root.Element("ModelingTask");
                var routinesEl = config.Root.Element("Routines");
                var orthogonalizationEl = modelingTaskEl.Element("Orthogonalization");

                System = modelingTaskEl.Element("TargetSystem").Value;
                Action = modelingTaskEl.Element("Action").Value;
                
                ActionParams = routinesEl.Descendants("Routine")
                    .First(e => e.Attribute("name").Value.Equals(Action, StringComparison.InvariantCultureIgnoreCase))
                    .Attribute("params").Value;
                
                Orthogonalization = orthogonalizationEl.Attribute("type").Value;
                OutDir = modelingTaskEl.Element("Output").Attribute("outDir").Value;
                BinOutput = Convert.ToBoolean(modelingTaskEl.Element("Output").Attribute("binaryOutput").Value);
                PicSize = new Size(
                    Convert.ToInt32(modelingTaskEl.Element("Output").Attribute("picWidth").Value), 
                    Convert.ToInt32(modelingTaskEl.Element("Output").Attribute("picHeight").Value));

                var systemEl = config.Root.Element("Systems").Descendants("System")
                    .First(e => e.Attribute("name").Value.Equals(System, StringComparison.InvariantCultureIgnoreCase));

                var parametersEl = systemEl.Element("Parameters");

                Iterations = int.Parse(orthogonalizationEl.Attribute("interval").Value, CultureInfo.InvariantCulture);

                var xSolver = systemEl.Element("Solver");

                SystemParameters.SystemName = System;
                SystemParameters.Solver = xSolver.Attribute("name").Value;
                SystemParameters.ModellingTime = Convert.ToDouble(xSolver.Attribute("time").Value);
                SystemParameters.Step = Convert.ToDouble(xSolver.Attribute("dt").Value);

                foreach (var paramEl in parametersEl.Descendants("Param"))
                {
                    var param = GetParameterFromXElement(paramEl);
                    param.Name = paramEl.Attribute("name").Value;

                    SystemParameters.ListParameters.Add(param);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to read params: " + ex);
            }
        }

        //System
        public SystemParameters SystemParameters { get; private set; } = new SystemParameters();

        //Modelling task
        public string Action { get; private set; }

        public string ActionParams { get; private set; }

        public string System { get; private set; }

        //normalization method
        public string Orthogonalization { get; private set; }

        //iterations between normalization
        public int Iterations { get; private set; }

        public string OutDir { get; private set; } = string.Empty;

        public bool BinOutput { get; private set; } = false;

        public Size PicSize { get; private set; }

        private Parameter GetParameterFromXElement(XElement el)
        {
            var parameter = new Parameter();

            string[] pair = Regex.Split(el.Attribute("range").Value, "\\.\\.");

            parameter.Start = Convert.ToDouble(pair[0].Trim());
            parameter.End = Convert.ToDouble(pair[1].Trim());
            parameter.Default = ParseParameterValue(el.Attribute("value").Value);
            return parameter;
        }

        private double ParseParameterValue(string value)
        {
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

        private double ParseValue(string value)
        {
            value = value.Trim();

            return value.Equals("pi", StringComparison.InvariantCultureIgnoreCase) ? 
                Math.PI : 
                Convert.ToDouble(value);
        }

        private double GetOperationResult(double val1, double val2, string operation)
        {
            switch(operation)
            {
                case "*":
                    return val1 * val2;
                case "/":
                    return val1 / val2;
                default:
                    throw new NotImplementedException($"operation {operation} is not recognized");
            }
        }
    }
}
