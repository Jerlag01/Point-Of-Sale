using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Pos.Views.NewTicket
{
    public partial class NewTicketPageView : Page, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        internal Grid BaseRowsGrid;
        internal Grid ContentRowGrid;
        internal Grid ProductsCategoriesGrid;
        internal Grid ProductsGridEmpty;
        internal Grid TouchMenuGrid;
        private bool _contentLoaded;

        public NewTicketPageView()
        {
            NewTicketPageView.logger.Debug("VIEW LOADING: NewTicketPageView");
            this.InitializeComponent();
        }

        ~NewTicketPageView()
        {
            NewTicketPageView.logger.Debug("VIEW DESTROYED: NewTicketPageView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/newticket/newticketpageview.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        internal Delegate _CreateDelegate(Type delegateType, string handler)
        {
            return Delegate.CreateDelegate(delegateType, (object)this, handler);
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.BaseRowsGrid = (Grid)target;
                    break;
                case 2:
                    this.ContentRowGrid = (Grid)target;
                    break;
                case 3:
                    this.ProductsCategoriesGrid = (Grid)target;
                    break;
                case 4:
                    this.ProductsGridEmpty = (Grid)target;
                    break;
                case 5:
                    this.TouchMenuGrid = (Grid)target;
                    break;
                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}
