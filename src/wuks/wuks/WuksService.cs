namespace wuks
{
    using System.ServiceProcess;

    internal class WuksService : ServiceBase
    {
        private WindowsUpdateKiller windowsUpdateKiller;

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            this.StartKiller();
        }

        protected override void OnStop()
        {
            base.OnStop();
            
            this.StopKiller();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.StartKiller();
            }
            
            base.Dispose(disposing);
        }

        private void StartKiller()
        {
            this.windowsUpdateKiller = new WindowsUpdateKiller();
            
            EventLogger.Info("Service started");
        }

        private void StopKiller()
        {
            this.windowsUpdateKiller?.Dispose();
            this.windowsUpdateKiller = null;
            
            EventLogger.Info("Service stopped");
        }
    }
}