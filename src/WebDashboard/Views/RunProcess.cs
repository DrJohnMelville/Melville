using System.Diagnostics;
using System.Threading.Tasks;

namespace WebDashboard.Views
{
    public interface IProcessProxy
    {
        bool HasExited { get; }
        Task<string?> ReadLineAsync();
    }

    public class ProcessProxy : IProcessProxy
    {
        private readonly Process process;

        public ProcessProxy(Process process)
        {
            this.process = process;
        }


        public bool HasExited => process.HasExited;

        public Task<string?> ReadLineAsync() => process.StandardOutput.ReadLineAsync();
    }
    
    public interface IRunProcess
    {
        IProcessProxy Run(string executable, string parameters);
    }

    public class RunProcess : IRunProcess
    {
        public IProcessProxy Run(string executable, string parameters)
        {
            var ret =  new Process()
            {
                StartInfo =new ProcessStartInfo(executable, parameters) 
                    {RedirectStandardOutput = true, 
                        UseShellExecute = false,
                        CreateNoWindow = true,  } 
            };
            ret.Start();

            return new ProcessProxy(ret);
        }
    }
}