using FileMoverWorkerService.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileMoverWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service started");
            return base.StartAsync(cancellationToken);  
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var f in ServiceConfiguration.WatchedFolder.GetFiles())
                    if (ServiceConfiguration.ExtensionsToMove.Any(x => $".{x}" == f.Extension))
                    {
                        f.MoveTo($@"{ServiceConfiguration.EndFolder.FullName}\{f.Name}");
                        _logger.LogInformation($"File {f.Name} was moved.");
                    }
                
                await Task.Delay(ServiceConfiguration.ServiceTimeLoopInSeconds, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stopped");
            return base.StopAsync(cancellationToken);
        }
    }
}
