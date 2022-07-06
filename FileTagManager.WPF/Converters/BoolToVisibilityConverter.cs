using System;
using System.Globalization;
using System.Windows;

namespace FileTagManager.WPF.Converters
{
    public class BoolToVisibilityConverter : BaseConverterr<BoolToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter == null
                ? (bool)value ? Visibility.Collapsed : Visibility.Visible
                : (object)((bool)value ? Visibility.Visible : Visibility.Collapsed);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
