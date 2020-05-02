using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Melville.Log.NamedPipeEventSink;
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
    public interface IHasTargetUrl
    {
        string TargetUrl { get; }
    }
    public class WelcomePageViewModel: NotifyBase, IHomeScreenPage, IHasTargetUrl
    {
        private IList<TargetSite> sites;
        public WelcomePageViewModel(IList<TargetSite> sites)
        {
            this.sites = sites;
        }

        public string Title => "Home Page";
        
        private string lastLogEntry ="";
        public string LastLogEntry
        {
            get => lastLogEntry;
            set => AssignAndNotify(ref lastLogEntry, value);
        }

        private string targetUrl = "";
        public string TargetUrl
        {
            get => targetUrl;
            set => AssignAndNotify(ref targetUrl, value);
        }


        private Random randomizer = new Random();
        private ILogger? logger;

        public void Stop()
        {
            // do nothing
        }

        public void ConnectLog()
        {
            logger = new LoggerConfiguration()
                .WriteTo.NamedPipe().CreateLogger();
        }

        public void Log() => logger?.Write((LogEventLevel)randomizer.Next((int) LogEventLevel.Fatal +1),
            "The Random Number is: {Number}", randomizer.Next(1000));

        public void LogException()
        {
            try
            {
                throw new Exception($"The number is {randomizer.Next(1000)}");
            }
            catch (Exception e)
            {
                logger?.Error(e, "Exception thrown");
            }
        }
    }
}