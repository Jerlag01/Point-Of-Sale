using System;
using System.Globalization;
using System.Windows.Data;

namespace Util.MicroMvvm.Converters
{
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || parameter == null ? (object)false : (object)value.ToString().Equals(parameter.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value == null || parameter == null)
                return (object)null;
            int num = (bool)value ? 1 : 0;
            string str = parameter.ToString();
            return num != 0 ? Enum.Parse(targetType, str) : (object)null;
        }
    }
}