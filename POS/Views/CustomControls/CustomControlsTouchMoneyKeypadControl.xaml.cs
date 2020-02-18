using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace Pos.Views.CustomControls
{
    public partial class TouchMoneyKeypadControl : UserControl, IComponentConnector
    {
        public static readonly DependencyProperty KeypadEntryProperty = DependencyProperty.Register(nameof(KeypadEntry), typeof(string), typeof(TouchMoneyKeypadControl), new PropertyMetadata((PropertyChangedCallback)null));
        public static readonly DependencyProperty EntryEnteredCommandProperty = DependencyProperty.Register(nameof(EntryEnteredCommand), typeof(ICommand), typeof(TouchMoneyKeypadControl));
        public static readonly DependencyProperty EnableDecimalEntryProperty = DependencyProperty.Register(nameof(EnableDecimalEntry), typeof(bool), typeof(TouchMoneyKeypadControl));
        internal Grid KeyPad;
        internal Grid Keys;
        private bool _contentLoaded;

        public TouchMoneyKeypadControl()
        {
            this.InitializeComponent();
            this.EnableDecimalEntry = true;
        }

        public string KeypadEntry
        {
            get
            {
                return (string)this.GetValue(TouchMoneyKeypadControl.KeypadEntryProperty);
            }
            set
            {
                this.SetValue(TouchMoneyKeypadControl.KeypadEntryProperty, (object)value);
            }
        }

        public ICommand EntryEnteredCommand
        {
            get
            {
                return (ICommand)this.GetValue(TouchMoneyKeypadControl.EntryEnteredCommandProperty);
            }
            set
            {
                this.SetValue(TouchMoneyKeypadControl.EntryEnteredCommandProperty, (object)value);
            }
        }

        public bool EnableDecimalEntry
        {
            get
            {
                return (bool)this.GetValue(TouchMoneyKeypadControl.EnableDecimalEntryProperty);
            }
            set
            {
                this.SetValue(TouchMoneyKeypadControl.EnableDecimalEntryProperty, (object)value);
            }
        }

        private void Value_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            string content = (string)((ContentControl)sender).Content;
            if (string.IsNullOrEmpty(content))
                return;
            if (content == "C")
            {
                if (string.IsNullOrEmpty(this.KeypadEntry))
                    return;
                this.KeypadEntry = this.KeypadEntry.Remove(this.KeypadEntry.Length - 1);
            }
            else if (content == ",")
            {
                if (this.KeypadEntry == null || this.KeypadEntry.Contains(","))
                    return;
                this.KeypadEntry += content;
            }
            else
            {
                if (!int.TryParse(content, out int _))
                    return;
                this.KeypadEntry += content;
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/customcontrols/touchmoneykeypadcontrol.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.KeyPad = (Grid)target;
                    break;
                case 2:
                    this.Keys = (Grid)target;
                    break;
                case 3:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 4:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 5:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 6:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 7:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 8:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 9:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 10:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 11:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 12:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 13:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                case 14:
                    ((ButtonBase)target).Click += new RoutedEventHandler(this.Value_Click);
                    break;
                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}
