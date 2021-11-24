namespace wuks
{
    using System;
    using System.ServiceProcess;
    using System.Timers;
    
    internal class WindowsUpdateKiller : IDisposable
    {
        private readonly Timer timer;

        public WindowsUpdateKiller()
        {
            this.timer = new Timer();
            this.timer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds;
            this.timer.Elapsed += this.OnTimerElapsed;
            this.timer.AutoReset = true;
            this.timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.TryStopWindowsUpdateService();
        }

        private void TryStopWindowsUpdateService()
        {
            try
            {
                var serviceController = new ServiceController("Windows Update");

                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Stop();
                    EventLogger.Info("Stopped Windows Update service");
                }
            }
            catch (Exception exception)
            {
                EventLogger.Error("Failed to stop Windows Update service", exception);   
            }
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }
    }
}