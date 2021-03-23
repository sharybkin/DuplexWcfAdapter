using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WcfTransmitter.Service.WcfEndpoint;

namespace WcfTransmitter.Service
{
    public class WcfService : ServiceBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public WcfService()
        {
            this.ServiceName = "WcfTransmitter";
        }

        protected override void OnStart(string[] args)
        {
            var hostRunner = new HostRunner<BettingLineService>();
            hostRunner.Run();
            Logger.Info("Wcf Host started");

            base.OnStart(args);
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStop()
        {
            Logger.Warn("Host stopped");
            base.OnStop();
        }
    }
}
