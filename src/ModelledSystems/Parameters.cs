using System;
using System.Xml.Linq;
using System.Globalization;
using System.Drawing;

namespace ModelledSystems
{
    class Parameters
    {

        //System
        public SystemParameters SystemParameters;

        //Modelling task
        public string Action;
        public string ActionParams;
        public string System;

        public string Orthogonalization;    //normalization method
        public int irate;                   //iterations between normalization
        public string OutDir = "";
        public Size PicSize;


        private string configFile = "systems_config.xml";

        private XDocument _configDoc = null;
        private XDocument configDoc {
            get {
                try {
                    _configDoc = XDocument.Load(configFile);
                }
                catch {
                    throw new ArgumentException("Unable to read configuration file " + configFile);
                }
                return _configDoc;
            }
        }


        private XElement _system = null;

        private XElement SystemEl
        {
            get
            {
                if (_system == null)
                {
                    string system = configDoc.Root.Element("ModelingTask").Element("TargetSystem").Value.ToLower();
                    XElement systems = configDoc.Root.Element("Systems");

                    foreach (XElement node in systems.Descendants("System"))
                        if (node.Attribute("name").Value.ToLower().Equals(system))
                        {
                            _system = node;
                            break;
                        }
                }

                return _system;
            }
        }

        public Parameters() {
            try
            {
                XElement modelingTask = configDoc.Root.Element("ModelingTask");

                Action = modelingTask.Element("Action").Attribute("name").Value;
                ActionParams = modelingTask.Element("Action").Value;
                System = modelingTask.Element("TargetSystem").Value;
                OutDir = modelingTask.Element("Output").Attribute("outDir").Value;

                PicSize = new Size(Convert.ToInt32(modelingTask.Element("Output").Attribute("picWidth").Value), Convert.ToInt32(modelingTask.Element("Output").Attribute("picHeight").Value));

                XElement orthogonalizationParams = modelingTask.Element("Orthogonalization");
                Orthogonalization = orthogonalizationParams.Attribute("type").Value;
                irate = int.Parse(orthogonalizationParams.Attribute("interval").Value, CultureInfo.InvariantCulture);


                SystemParameters = new SystemParameters();
                SystemParameters.SystemName = System;
                SystemParameters.ModellingTime = Convert.ToDouble(SystemEl.Element("ModellingTime").Value);

                Parameter stepParam = new Parameter();
                stepParam.Start = Convert.ToDouble(SystemEl.Element("Parameters").Element("Step").Attribute("start").Value);
                stepParam.Step = Convert.ToDouble(SystemEl.Element("Parameters").Element("Step").Attribute("step").Value);
                stepParam.End = Convert.ToDouble(SystemEl.Element("Parameters").Element("Step").Attribute("end").Value);
                stepParam.Default = Convert.ToDouble(SystemEl.Element("Parameters").Element("Step").Attribute("default").Value);
                SystemParameters.Step = stepParam;

                foreach (XElement paramEl in SystemEl.Element("Parameters").Descendants("Param"))
                {
                    Parameter param = new Parameter();
                    param.Name = paramEl.Attribute("name").Value;
                    param.Start = Convert.ToDouble(paramEl.Attribute("start").Value);
                    param.Step = Convert.ToDouble(paramEl.Attribute("step").Value);
                    param.End = Convert.ToDouble(paramEl.Attribute("end").Value);
                    param.Default = Convert.ToDouble(paramEl.Attribute("default").Value);
                    SystemParameters.ListParameters.Add(param);
                }

            }
            catch (Exception Exception) {
                throw new ArgumentException("Unable to read params: " + Exception.Message + Exception.StackTrace);
            }
        }
        
    }
}
