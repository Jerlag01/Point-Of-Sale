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
    public partial class MembercardConfigControlView : UserControl, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        internal Grid SettingsGrid;
        private bool _contentLoaded;

        public MembercardConfigControlView()
        {
            MembercardConfigControlView.logger.Debug("VIEW LOADING: MembercardConfigControlView");
            this.InitializeComponent();
        }

        ~MembercardConfigControlView()
        {
            MembercardConfigControlView.logger.Debug("VIEW DESTROYED: MembercardConfigControlView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/settings/membercardconfigcontrolview.xaml", UriKind.Relative));
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