using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WcfTransmitter.Service
{
    public class HostRunner<T>
    {
        private ServiceHost _host;
        private readonly TimeSpan _timeout = TimeSpan.FromMinutes(5d);
        private Timer _timer;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public void Run()
        {
            RunCore();
        }

        void RunCore()
        {

            ServiceHost host;

            try
            {
                host = new ServiceHost(typeof(T));
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "An error occurred while creating the service host");
                RunLater();
                return;
            }

            var previousHost = Interlocked.Exchange(ref _host, host);
            if (previousHost != null)
                Close(previousHost);

            host.Faulted += OnHostFault;
            host.Closed += OnHostClosed;

            if (host.State == CommunicationState.Created)
            {
                try
                {
                    host.Open();
                }
                catch (ObjectDisposedException)
                {
                    RunLater();
                    return;
                }
                catch (CommunicationObjectFaultedException)
                {
                    RunLater();
                    return;
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Failed to start the service host {0}.", host.Description.ServiceType);

                    RunLater();
                    return;
                }
            }

            if (host.State != CommunicationState.Opening && host.State != CommunicationState.Opened)
                RunLater();
        }

        void RunLater()
        {
            var previous = Interlocked.Exchange(ref _timer, new Timer(state => Run(), null, _timeout, System.Threading.Timeout.InfiniteTimeSpan));
            previous?.Dispose();
        }

        void Close(ServiceHost host)
        {
            host.Faulted -= OnHostFault;
            host.Closed -= OnHostClosed;

            if (host.State != CommunicationState.Faulted)
            {
                try
                {
                    host.Close();
                    return;
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (InvalidOperationException)
                {
                    return;
                }
                catch (CommunicationObjectFaultedException)
                {
                }
                catch (Exception exception)
                {
                    _logger.Warn(exception, "Failed to close the service host  {0}.", host.Description.ServiceType);
                }
            }

            Abort(host);
        }

        void Abort(ServiceHost host)
        {
            try
            {
                host.Abort();
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Failed to force close the service host {0}.", host.Description.ServiceType);
            }
        }

        void OnHostFault(object sender, EventArgs e)
        {

            var host = Interlocked.CompareExchange(ref _host, null, (ServiceHost)sender);

            if (host != null)
            {
                RunLater();
                Close(host);
            }
        }

        void OnHostClosed(object sender, EventArgs e)
        {
            var host = Interlocked.CompareExchange(ref _host, null, (ServiceHost)sender);

            if (host != null)
            {
                RunLater();
                Close(host);
            }
        }
    }
}
