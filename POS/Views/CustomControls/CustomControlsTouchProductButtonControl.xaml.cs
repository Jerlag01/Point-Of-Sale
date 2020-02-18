using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Dal.Model;
using Util;

namespace Pos.Views.CustomControls
{
    public partial class TouchProductButtonControl : UserControl, IComponentConnector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly DependencyProperty ProductProperty = DependencyProperty.Register(nameof(Product), typeof(Product), typeof(TouchProductButtonControl), new PropertyMetadata(new PropertyChangedCallback(TouchProductButtonControl.OnProductChanged)));
        private static readonly DependencyProperty ClickCommandParameterProperty = DependencyProperty.Register(nameof(ClickCommandParameter), typeof(Product), typeof(TouchProductButtonControl), new PropertyMetadata((PropertyChangedCallback)null));
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(TouchProductButtonControl));
        internal Image ImageElement;
        private bool _contentLoaded;

        public TouchProductButtonControl()
        {
            this.InitializeComponent();
        }

        private ImageSource ImageSource
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

        private static void OnProductChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TouchProductButtonControl productButtonControl) || productButtonControl.Product == null || productButtonControl.Product.PicturePath == null)
                return;
            string path = "Images/" + productButtonControl.Product.PicturePath;
            try
            {
                productButtonControl.ImageSource = (ImageSource)ImageHelper.LoadPngImage(path);
            }
            catch (ArgumentException ex)
            {
                TouchProductButtonControl.logger.Warn<string, string>("File not found: {0}, {1}", ex.Message, path);
            }
        }

        public Product Product
        {
            get
            {
                return (Product)this.GetValue(TouchProductButtonControl.ProductProperty);
            }
            set
            {
                this.SetValue(TouchProductButtonControl.ProductProperty, (object)value);
            }
        }

        public Product ClickCommandParameter
        {
            get
            {
                return (Product)this.GetValue(TouchProductButtonControl.ClickCommandParameterProperty);
            }
            set
            {
                this.SetValue(TouchProductButtonControl.ClickCommandParameterProperty, (object)value);
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return (ICommand)this.GetValue(TouchProductButtonControl.ClickCommandProperty);
            }
            set
            {
                this.SetValue(TouchProductButtonControl.ClickCommandProperty, (object)value);
            }
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/Pos;component/views/customcontrols/touchproductbuttoncontrol.xaml", UriKind.Relative));
        }

        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
                this.ImageElement = (Image)target;
            else
                this._contentLoaded = true;
        }
    }
}
