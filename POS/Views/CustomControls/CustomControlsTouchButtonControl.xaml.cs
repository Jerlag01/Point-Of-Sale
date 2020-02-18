using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Pos.Views.CustomControls
{
    public partial class TouchButtonControl : UserControl, IComponentConnector
    {
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(TouchButtonControl));
        internal Button ButtonElement;
        internal Image ImageElement;
        internal TextBlock TextElement;
        private bool _contentLoaded;

        public TouchButtonControl()
        {
            this.InitializeComponent();
        }

        public string ButtonText
        {
            get
            {
                return this.TextElement.Text;
            }
            set
            {
                this.TextElement.Text = value;
            }
        }

        public Style ButtonStyle
        {
            get
            {
                return this.ButtonElement.Style;
            }
            set
            {
                this.ButtonElement.Style = value;
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                return this.ImageElement.Source;
            }
            set
            {
                this.ImageElement.Source = value;
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return (ICommand)this.GetValue(TouchButtonControl.ClickCommandProperty);
            }
            set
            {
                this.SetValue(TouchButtonControl.ClickCommandProperty, (object)value);
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/customcontrols/touchbuttoncontrol.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.ButtonElement = (Button)target;
                    break;
                case 2:
                    this.ImageElement = (Image)target;
                    break;
                case 3:
                    this.TextElement = (TextBlock)target;
                    break;
                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}
