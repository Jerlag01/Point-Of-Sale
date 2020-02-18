using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Pos.Views.Settings
{
    public partial class MailConfigControlView : UserControl, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        internal Grid SettingsGrid;
        private bool _contentLoaded;

        public MailConfigControlView()
        {
            MailConfigControlView.logger.Debug("VIEW LOADING: MailConfigControlView");
            this.InitializeComponent();
        }

        ~MailConfigControlView()
        {
            MailConfigControlView.logger.Debug("VIEW DESTROYED: MailConfigControlView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/settings/mailconfigcontrolview.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
                this.SettingsGrid = (Grid)target;
            else
                this._contentLoaded = true;
        }
    }
}