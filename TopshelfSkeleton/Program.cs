using System;
using Topshelf;

namespace TopshelfSkeleton
{
    class Program : ServiceControl
    {
        static void Main(string[] args)
        {
            HostFactory.New(x =>
            {
                x.SetServiceName("ServiceName");
                x.SetDescription("ServiceDescription");
                x.SetDisplayName("ServiceDisplayName");
                x.StartAutomaticallyDelayed();
                x.Service<Program>();
            }).Run();
        }

        public bool Start(HostControl hostControl)
        {
            throw new NotImplementedException();
        }

        public bool Stop(HostControl hostControl)
        {
            throw new NotImplementedException();
        }
    }
}
