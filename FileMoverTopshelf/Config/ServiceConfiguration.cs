using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMoverTopshelf.Config
{
    public static class ServiceConfiguration
    {
        public static DirectoryInfo WatchedFolder { get; private set; }
        public static DirectoryInfo EndFolder { get; private set; }
        public static List<string> ExtensionsToMove { get; private set; }
        public static int ServiceTimeLoopInSeconds { get; private set; }
        public static string LogFileFullName { get; private set; }

        static ServiceConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            WatchedFolder = new DirectoryInfo(config["AppSettings:WatchedFolder"]);
            EndFolder = new DirectoryInfo(config["AppSettings:EndFolder"]);
            ExtensionsToMove = new List<string>(config["AppSettings:ExtensionsToMove"].Split(new char[] { ';' }));
            ServiceTimeLoopInSeconds = int.Parse(config["AppSettings:ServiceTimeLoopInSeconds"]);
            LogFileFullName = config["AppSettings:LogFileFullName"];
        }
    }
}
