using System;
using System.IO;
using System.Threading.Tasks;
using Melville.Log.Viewer.EventSink;
using Melville.Log.Viewer.HomeScreens;
using Melville.Log.Viewer.NamedPipeServers;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.DiParameterSources;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Melville.Log.Viewer.WelcomePage
{
    public class WelcomePageViewModel: NotifyBase, IHomeScreenPage
    {
        public string Title => "Home Page";


        private string lastLogEntry ="";
        public string LastLogEntry
        {
            get => lastLogEntry;
            set => AssignAndNotify(ref lastLogEntry, value);
        }

        private Random randomizer = new Random();
        private ILogger? logger;
        public WelcomePageViewModel()
        {
        }

        public void ConnectLog([FromServices] Func<NamedPipeEventSink> createSink)
        {
            logger = new LoggerConfiguration()
                .WriteTo.Sink(createSink()).CreateLogger();
        }


        public void Log() => logger?.Error("The Random Number is: {Number}", randomizer.Next(1000));
    }
}