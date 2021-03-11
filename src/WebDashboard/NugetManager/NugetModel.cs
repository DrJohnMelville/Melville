namespace WebDashboard.NugetManager
{
    public class NugetModel
    {
        public NugetModel(string version)
        {
            Version = version;
        }

        public string Version { get; }
    }
}