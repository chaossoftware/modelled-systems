using System;
using System.Linq;

namespace ModelledSystems.Configuration;

internal static class ConfigUtils
{
    internal static double ParseParameterValue(string value)
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
