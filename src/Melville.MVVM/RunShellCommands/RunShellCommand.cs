using System.Diagnostics;

namespace Melville.MVVM.RunShellCommands
{
    public interface IRunShellCommand
    {
        void ShellExecute(string command, params string[] parameters);
        IProcessProxy CapturedShellExecute(string command, params string[] parameters);
    }
    
    public class RunShellCommand: IRunShellCommand
    {
        public void ShellExecute(string command, params string[] parameters)
        {
            var psi = CreateInfo(command, parameters);
            psi.Verb = "";
            psi.UseShellExecute = true;
            StartProcess(psi);
        }

        public IProcessProxy CapturedShellExecute(string command, params string[] parameters) 
        {
            var psi = CreateInfo(command, parameters);
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;
            return new ProcessProxy(StartProcess(psi));
        }

        private Process StartProcess(ProcessStartInfo psi)
        {
            var proc = new Process
            {
                StartInfo = psi
            };
            proc.Start();
            return proc;
        }

        private ProcessStartInfo CreateInfo(string command, string[] parameters) =>
            new(command, CombineParameters(parameters));

        private string CombineParameters(string[] parameters) => string.Join(" ", parameters);
    }
}