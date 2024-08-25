using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.WPF.View.Guide.Converters
{
    public class DayTimeConverter: IValueConverter
    {

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value is string dateTimeString)
            {
                if (DateTime.TryParse(dateTimeString, out DateTime dateTime))
                {
                    return dateTime.ToString("HH:mm:ss");
                }
            }
            return value;
        }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
}
