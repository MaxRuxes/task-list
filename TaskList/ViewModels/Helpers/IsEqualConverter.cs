using System;
using System.Globalization;
using System.Windows.Data;

namespace TaskList.ViewModels.Helpers
{
    public class IsEqualConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsEqualConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intValue = (int) value;
            var compareToValue = int.Parse(parameter as string);

            return intValue == compareToValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
