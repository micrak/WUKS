namespace wuks
{
    using System.ServiceProcess;

    public class WuksServiceInstaller : ServiceInstaller
    {
        public WuksServiceInstaller()
        {
            this.ServiceName = Constants.ServiceName;
            this.Description = Constants.ServiceDescription;
            this.StartType = ServiceStartMode.Automatic;
        }
    }
}