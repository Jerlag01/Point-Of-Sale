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
    public partial class ApplicationConfigControlView : UserControl, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool _contentLoaded;

        public ApplicationConfigControlView()
        {
            ApplicationConfigControlView.logger.Debug("VIEW LOADING: ApplicationConfigControlView");
            this.InitializeComponent();
        }

        ~ApplicationConfigControlView()
        {
            ApplicationConfigControlView.logger.Debug("VIEW DESTROYED: ApplicationConfigControlView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/settings/applicationconfigcontrolview.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }
    }
}