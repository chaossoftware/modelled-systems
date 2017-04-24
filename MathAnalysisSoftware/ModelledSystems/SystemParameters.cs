using System.Collections.Generic;

namespace ModelledSystems
{
    public class SystemParameters
    {
        public double ModellingTime;
        public string SystemName;

        public Parameter Step;
        public List<Parameter> ListParameters;

        public SystemParameters()
        {
            ListParameters = new List<Parameter>();
        }

        public double[] Defaults
        {
            get
            {
                double[] def = new double[ListParameters.Count];
                for (int i = 0; i < ListParameters.Count; i++)
                    def[i] = ListParameters[i].Default;

                return def;
            }
        }
    }

    public class Parameter
    {
        public double Start;
        public double Step;
        public double End;
        public double Default;
        public string Name;
    }
}
