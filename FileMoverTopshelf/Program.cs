using FileMoverTopshelf.Config;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace FileMoverTopshelf
{
    class Program : ServiceControl
    {
        private static TimeSpan _fileMoverServiceInterval;
        private ManualResetEvent _manualResetEvent;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(ServiceConfiguration.LogFileFullName,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level}] {Message} {NewLine}")
                .CreateLogger();

            HostFactory.New(x =>
            {
                x.SetServiceName("ServiceName");
                x.SetDescription("ServiceDescription");
                x.SetDisplayName("ServiceDisplayName");
                x.StartAutomaticallyDelayed();
                x.UseSerilog();
                x.Service<Program>();
            }).Run();
        }

        public bool Start(HostControl hostControl)
        {
            _fileMoverServiceInterval = TimeSpan.FromSeconds(ServiceConfiguration.ServiceTimeLoopInSeconds);
            _manualResetEvent = new ManualResetEvent(false);

            Log.Information("Topshelf service started.");

            var t = new Task(() =>
            {
                Execute();
            });

            t.Start();

            return true;
        }

        protected void Execute()
        {
            do
            {
                foreach (var f in ServiceConfiguration.WatchedFolder.GetFiles())
                    if (ServiceConfiguration.ExtensionsToMove.Any(x => $".{x}" == f.Extension))
                    {
                        f.MoveTo($@"{ServiceConfiguration.EndFolder.FullName}\{f.Name}");
                        Log.Information($"File {f.Name} was moved.");
                    }

            } while (_manualResetEvent.WaitOne(_fileMoverServiceInterval));
        }

        public bool Stop(HostControl hostControl)
        {
            Log.Information("Topshelf service stopped.");
            _manualResetEvent.Set();
            return true;
        }
    }
}
