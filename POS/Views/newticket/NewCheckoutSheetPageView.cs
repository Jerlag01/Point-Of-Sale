﻿using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Pos.Views.NewTicket
{
    public partial class NewCheckoutSheetPageView : Page, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        internal Grid BaseRowsGrid;
        internal Grid ContentRowGrid;
        internal Grid TouchMenuGrid;
        private bool _contentLoaded;

        public NewCheckoutSheetPageView()
        {
            NewCheckoutSheetPageView.logger.Debug("VIEW LOADING: NewCheckoutSheetView");
            this.InitializeComponent();
        }

        ~NewCheckoutSheetPageView()
        {
            NewCheckoutSheetPageView.logger.Debug("VIEW DESTROYED: NewCheckoutSheetView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/newticket/newcheckoutsheetpageview.xaml", UriKind.Relative));
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
                    this.TouchMenuGrid = (Grid)target;
                    break;
                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}
