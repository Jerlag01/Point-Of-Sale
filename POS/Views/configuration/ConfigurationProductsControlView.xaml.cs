using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Pos.Views.Configuration
{
    public partial class ProductsControlView : UserControl, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        internal Grid ProductsCategoriesGrid;
        private bool _contentLoaded;

        public ProductsControlView()
        {
            ProductsControlView.logger.Debug("VIEW LOADING: ProductsControlView");
            this.InitializeComponent();
        }

        ~ProductsControlView()
        {
            ProductsControlView.logger.Debug("VIEW DESTROYED: ProductsControlView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/configuration/productscontrolview.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
                this.ProductsCategoriesGrid = (Grid)target;
            else
                this._contentLoaded = true;
        }
    }
}