using System.Windows;
using System.Windows.Controls;

namespace Util.WpfMessageBox
{
    public class WPFMessageBoxControl : Control
    {
        static WPFMessageBoxControl()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(WPFMessageBoxControl), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(WPFMessageBoxControl)));
        }
    }
}