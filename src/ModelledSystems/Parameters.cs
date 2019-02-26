using System;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Drawing;

namespace ModelledSystems
{
    internal class Parameters
    {
        private const string ConfigFile = "systems_config.xml";

        public Parameters()
        {
            try
            {
                var config = XDocument.Load(ConfigFile);
                var modelingTaskEl = config.Root.Element("ModelingTask");
                var orthogonalizationEl = modelingTaskEl.Element("Orthogonalization");

                System = modelingTaskEl.Element("TargetSystem").Value;
                Action = modelingTaskEl.Element("Action").Attribute("name").Value;
                ActionParams = modelingTaskEl.Element("Action").Value;
                Orthogonalization = orthogonalizationEl.Attribute("type").Value;
                OutDir = modelingTaskEl.Element("Output").Attribute("outDir").Value;
                PicSize = new Size(Convert.ToInt32(modelingTaskEl.Element("Output").Attribute("picWidth").Value), Convert.ToInt32(modelingTaskEl.Element("Output").Attribute("picHeight").Value));

                var systemEl = config.Root.Element("Systems").Descendants("System")
                    .First(e => e.Attribute("name").Value.Equals(System, StringComparison.InvariantCultureIgnoreCase));

                var parametersEl = systemEl.Element("Parameters");

                Iterations = int.Parse(orthogonalizationEl.Attribute("interval").Value, CultureInfo.InvariantCulture);

                SystemParameters.SystemName = System;
                SystemParameters.ModellingTime = Convert.ToDouble(systemEl.Element("ModellingTime").Value);

                var stepParam = GetParameterFromXElement(parametersEl.Element("Step"));
                SystemParameters.Step = stepParam;

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

        public Size PicSize { get; private set; }

        private Parameter GetParameterFromXElement(XElement el)
        {
            var parameter = new Parameter();
            parameter.Start = Convert.ToDouble(el.Attribute("start").Value);
            parameter.Step = Convert.ToDouble(el.Attribute("step").Value);
            parameter.End = Convert.ToDouble(el.Attribute("end").Value);
            parameter.Default = Convert.ToDouble(el.Attribute("default").Value);
            return parameter;
        }
    }
}
