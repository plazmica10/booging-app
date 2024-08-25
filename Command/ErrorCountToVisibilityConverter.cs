using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BookingApp.Command
{
    public class ErrorCountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int errorCount = (int)value;
            return errorCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
