using System;
using System.ServiceProcess;

namespace WcfTransmitter.Service
{
    public static class WcfServiceRunner
    {
        public static void Run(params string[] args)
        {
            if (Environment.UserInteractive)
            {
                var service = new WcfService();
                service.Start(args);

                if (!string.IsNullOrEmpty(service.ServiceName))
                    Console.Title = service.ServiceName;

                Console.WriteLine("Press enter to exit...");
                Console.ReadLine();
                service.Stop();
                Environment.Exit(0);
            }
            else
            {
                var service = new WcfService();
                ServiceBase.Run(service);
            }
        }
    }
}
