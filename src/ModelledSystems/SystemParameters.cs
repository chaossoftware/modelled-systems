using System.Collections.Generic;
using System.Linq;

namespace ModelledSystems
{
    public struct Parameter
    {
        public double Start { get; set; }

        public double Step { get; set; }

        public double End { get; set; }

        public double Default { get; set; }

        public string Name { get; set; }
    }

    public class SystemParameters
    {
        public SystemParameters()
        {
            ListParameters = new List<Parameter>();
        }

        public double ModellingTime { get; set; }

        public string SystemName { get; set; }

        public Parameter Step { get; set; }
        public List<Parameter> ListParameters { get; set; }

        public double[] Defaults => ListParameters.Select(p => p.Default).ToArray();
    }
}
