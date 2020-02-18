using System;
using System.Windows;
using System.Windows.Controls;

namespace Util
{
    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty = DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), (PropertyMetadata)new UIPropertyMetadata((object)null, new PropertyChangedCallback(WebBrowserUtility.BindableSourcePropertyChanged)));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(WebBrowserUtility.BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(WebBrowserUtility.BindableSourceProperty, (object)value);
        }

        public static void BindableSourcePropertyChanged(
            DependencyObject o,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(o is WebBrowser webBrowser))
                return;
            string newValue = e.NewValue as string;
            Uri uri = !string.IsNullOrEmpty(newValue) ? new Uri(newValue) : (Uri)null;
            webBrowser.Source = uri;
        }
    }
}
