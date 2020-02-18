using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Pos.Views
{
    public partial class SplashScreenView : Window, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        internal MediaTypeNames.Image ImgBrush;
        internal DoubleAnimation FadeIn;
        private bool _contentLoaded;

        public SplashScreenView()
        {
            this.WindowState = WindowState.Maximized;
            SplashScreenView.logger.Debug("VIEW LOADING: SplashScreenView");
            this.InitializeComponent();
        }

        ~SplashScreenView()
        {
            SplashScreenView.logger.Debug("VIEW DESTROYED: SplashScreenView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            MediaTypeNames.Application.LoadComponent((object)this, new Uri("/Pos;component/views/splashscreenview.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId != 1)
            {
                if (connectionId == 2)
                    this.FadeIn = (DoubleAnimation)target;
                else
                    this._contentLoaded = true;
            }
            else
                this.ImgBrush = (Image)target;
        }
    }
}