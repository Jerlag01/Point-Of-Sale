﻿using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Pos.Views.NewMembercard
{
    public partial class ExistingMembercardControlView : UserControl, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool _contentLoaded;

        public ExistingMembercardControlView()
        {
            ExistingMembercardControlView.logger.Debug("VIEW LOADING: ExistingMembercardControlView");
            this.InitializeComponent();
        }

        ~ExistingMembercardControlView()
        {
            ExistingMembercardControlView.logger.Debug("VIEW DESTROYED: ExistingMembercardControlView");
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/newmembercard/existingmembercardcontrolview.xaml", UriKind.Relative));
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
            this._contentLoaded = true;
        }
    }
}
