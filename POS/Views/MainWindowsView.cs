using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Pos.Views
{
    public partial class MainWindowView : Window, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        internal Grid BaseGrid;
        internal Grid MenuBarGrid;
        internal System.Windows.Controls.Frame MainFrame;
        private bool _contentLoaded;

        public MainWindowView()
        {
            this.WindowState = WindowState.Maximized;
            MainWindowView.logger.Debug("VIEW LOADING: MainWindowView");
            this.InitializeComponent();
        }

        ~MainWindowView()
        {
            MainWindowView.logger.Debug("VIEW DESTROYED: MainWindowView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/mainwindowview.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.BaseGrid = (Grid)target;
                    break;
                case 2:
                    this.MenuBarGrid = (Grid)target;
                    break;
                case 3:
                    this.MainFrame = (System.Windows.Controls.Frame)target;
                    break;
                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}
