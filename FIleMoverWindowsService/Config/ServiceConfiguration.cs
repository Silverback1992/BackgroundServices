using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIleMoverWindowsService.Config
{
    public static class ServiceConfiguration
    {
        public static DirectoryInfo WatchedFolder { get; private set; }
        public static DirectoryInfo EndFolder { get; private set; }
        public static List<string> ExtensionsToMove { get; private set; }
        public static double ServiceTimeLoopInSeconds { get; private set; }
        public static string LogFileFullName { get; private set; }

        static ServiceConfiguration()
        {
            WatchedFolder = new DirectoryInfo(ConfigurationManager.AppSettings["WatchedFolder"]);
            EndFolder = new DirectoryInfo(ConfigurationManager.AppSettings["EndFolder"]);
            ExtensionsToMove = new List<string>(ConfigurationManager.AppSettings["ExtensionsToMove"].Split(new char[] {';'}));
            ServiceTimeLoopInSeconds = double.Parse(ConfigurationManager.AppSettings["ServiceTimeLoopInSeconds"]);
            LogFileFullName = ConfigurationManager.AppSettings["LogFileFullName"];
        }
    }
}
