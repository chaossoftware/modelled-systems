namespace ModelledSystems;

internal static class SysFormat
{
    /// <summary>
    /// Gets template string for file name using system parameters names.
    /// </summary>
    /// <param name="paramNames">system parameters names</param>
    /// <returns></returns>
    internal static string GetFileTemplate(string name, params string[] paramNames)
    {
        string template = $"{name}";

        for (int i = 0; i < paramNames.Length; i++)
        {
            template += $"_{paramNames[i]}={{{i}:0.###}}";
        }

        return template;
    }

    /// <summary>
    /// Gets template string for system info using system parameters names.
    /// </summary>
    /// <param name="paramNames">system parameters names</param>
    /// <returns></returns>
    internal static string GetInfoTemplate(string name, params string[] paramNames)
    {
        string template = $"{name}:";

        for (int i = 0; i < paramNames.Length; i++)
        {
            template += $" {paramNames[i]}={{{i}:G6}}";
        }

        return template;
    }
}
