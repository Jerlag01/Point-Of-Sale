using NLog;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Util.MicroMvvm.Converters
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class ImageConverter : IValueConverter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            string path = "Images\\" + (string)value;
            try
            {
                return (object)ImageHelper.LoadPngImage(path);
            }
            catch (ArgumentException ex)
            {
                ImageConverter.logger.Warn<string, string>("File not found: {0}, {1}", ex.Message, path);
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}