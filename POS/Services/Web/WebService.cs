using Microsoft.Owin.Hosting;
using System.ComponentModel;
using System.Data.SqlClient;
using System.ServiceProcess;

namespace Pos.Services.Web
{
    public class WebService : ServiceBase
    {
        private IContainer components;

        public WebService()
        {
            this.InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Dal.Settings.ConnectionString = new SqlConnectionStringBuilder()
            {
                Password = Pos.Services.Web.Properties.Settings.Default.DbPassword,
                UserID = Pos.Services.Web.Properties.Settings.Default.DbUser,
                InitialCatalog = Pos.Services.Web.Properties.Settings.Default.DbCatalog,
                DataSource = Pos.Services.Web.Properties.Settings.Default.DbInstance
            }.ToString();
            WebApp.Start<Startup>("http://+:9000/");
        }

        protected override void OnStop()
        {
        }

        public void onDebug()
        {
            this.OnStart((string[])null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ServiceName = "POS WebService";
        }
    }
}