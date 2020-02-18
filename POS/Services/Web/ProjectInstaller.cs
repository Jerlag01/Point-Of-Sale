using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Pos.Services.Web
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private IContainer components;
        private ServiceProcessInstaller serviceProcessInstaller1;
        private ServiceInstaller serviceInstaller1;

        public ProjectInstaller()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new ServiceProcessInstaller();
            this.serviceInstaller1 = new ServiceInstaller();
            this.serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = (string)null;
            this.serviceProcessInstaller1.Username = (string)null;
            this.serviceInstaller1.ServiceName = "POS WebService";
            this.serviceInstaller1.StartType = ServiceStartMode.Automatic;
            this.Installers.AddRange(new Installer[2]
            {
                (Installer) this.serviceProcessInstaller1,
                (Installer) this.serviceInstaller1
            });
        }
    }
}