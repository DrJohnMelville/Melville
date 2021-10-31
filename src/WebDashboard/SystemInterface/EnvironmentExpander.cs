using System;

namespace WebDashboard.SystemInterface;

public interface IEnvironmentExpander
{
    string Expand(string input);
}

public class EnvironmentExpander: IEnvironmentExpander
{
    public string Expand(string input)
    {
        return Environment.ExpandEnvironmentVariables(input);
    }
}