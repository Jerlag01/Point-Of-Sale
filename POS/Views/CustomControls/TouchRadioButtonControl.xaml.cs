using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Pos.Views.CustomControls
{
    public partial class TouchRadioButtonControl : UserControl, IComponentConnector
    {
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool?), typeof(TouchRadioButtonControl), new PropertyMetadata((PropertyChangedCallback)null));
        internal RadioButton RadioButtonElement;
        internal Image ImageElement;
        internal TextBlock TextElement;
        private bool _contentLoaded;

        public TouchRadioButtonControl()
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

        public string GroupName
        {
            get
            {
                return this.RadioButtonElement.GroupName;
            }
            set
            {
                this.RadioButtonElement.GroupName = value;
            }
        }

        public Style ButtonStyle
        {
            get
            {
                return this.RadioButtonElement.Style;
            }
            set
            {
                this.RadioButtonElement.Style = value;
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

        public double ImageWidth
        {
            get
            {
                return this.ImageElement.Width;
            }
            set
            {
                this.ImageElement.Width = value;
            }
        }

        public bool? IsChecked
        {
            get
            {
                return (bool?)this.GetValue(TouchRadioButtonControl.IsCheckedProperty);
            }
            set
            {
                this.SetValue(TouchRadioButtonControl.IsCheckedProperty, (object)value);
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/customcontrols/touchradiobuttoncontrol.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.RadioButtonElement = (RadioButton)target;
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
