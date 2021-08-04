using FIleMoverWindowsService.Config;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FIleMoverWindowsService
{
    public partial class FileMover : ServiceBase
    {
        private static TimeSpan _fileMoverServiceInterval;
        private ManualResetEvent _manualResetEvent; 

        public FileMover()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Debugger.Launch();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(ServiceConfiguration.LogFileFullName,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level}] {Message} {NewLine}")
                .CreateLogger();

            Log.Information("File Mover Windows Service started.");

            _fileMoverServiceInterval = TimeSpan.FromSeconds(ServiceConfiguration.ServiceTimeLoopInSeconds);
            _manualResetEvent = new ManualResetEvent(false);

            var t = new Task(() =>
            {
                do
                {
                    foreach (var f in ServiceConfiguration.WatchedFolder.GetFiles())
                        if (ServiceConfiguration.ExtensionsToMove.Any(x => $".{x}" == f.Extension))
                        {
                            f.MoveTo($@"{ServiceConfiguration.EndFolder.FullName}\{f.Name}");
                            Log.Information($"File {f.Name} was moved.");
                        }

                } while (!_manualResetEvent.WaitOne(_fileMoverServiceInterval));
            });

            t.Start();
        }

        protected override void OnStop()
        {
            Log.Information("File Mover Windows Service stopped.");
        }
    }
}
