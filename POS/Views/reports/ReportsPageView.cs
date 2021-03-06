﻿using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Pos.Views.Reports
{
    public partial class ReportsPageView : Page, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        internal Grid ContentRowGrid;
        internal Grid TouchMenuGrid;
        private bool _contentLoaded;

        public ReportsPageView()
        {
            ReportsPageView.logger.Debug("VIEW LOADING: ReportsPageView");
            this.InitializeComponent();
        }

        ~ReportsPageView()
        {
            ReportsPageView.logger.Debug("VIEW DESTROYED: ReportsPageView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/reports/reportspageview.xaml", UriKind.Relative));
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
            if (connectionId != 1)
            {
                if (connectionId == 2)
                    this.TouchMenuGrid = (Grid)target;
                else
                    this._contentLoaded = true;
            }
            else
                this.ContentRowGrid = (Grid)target;
        }
    }
}
