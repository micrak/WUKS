namespace wuks
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ServiceProcess;
    
    [RunInstaller(true)]
    public class WuksServiceProcessInstaller : ServiceProcessInstaller
    {
        public WuksServiceProcessInstaller()
        {
            this.Account = ServiceAccount.LocalSystem;
            this.Installers.Add(new WuksServiceInstaller());
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            try
            {
                var serviceController = new ServiceController(Constants.ServiceName);
                serviceController.Start();
            }
            catch (Exception exception)
            {
                EventLogger.Error("Failed to start the service after installation.", exception);
            }
        }
    }
}