using System;
using System.Globalization;

namespace FileTagManager.WPF.Converters
{
    public class IntToBoolConverter : BaseConverterr<IntToBoolConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (int)value > 0;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => true;
    }
}
