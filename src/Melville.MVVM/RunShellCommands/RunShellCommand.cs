namespace Melville.MVVM.RunShellCommands
{
    public interface IRunShellCommand
    {
        void ShellExecute(string command, params string[] parameters);
    }
    public class RunShellCommand: IRunShellCommand
    {
        public void ShellExecute(string command, params string[] parameters)
        {
            var proc = new System.Diagnostics.Process
            {
                StartInfo = {FileName = command, 
                    Arguments = string.Join(" ", parameters), 
                    Verb = "", 
                    UseShellExecute = true}
            };
            proc.Start();
        }
    }
}