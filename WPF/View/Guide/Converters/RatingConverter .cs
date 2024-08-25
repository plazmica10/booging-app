using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace BookingApp.WPF.View.Guide.Converters
{
    public class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int rating)
            {
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

                SolidColorBrush yellowBrush = new SolidColorBrush(Colors.Tomato);
                SolidColorBrush whiteBrush = new SolidColorBrush(Colors.LightGray);

                for (int i = 0; i < 5; i++)
                {
                    TextBlock star = new TextBlock
                    {
                        Text = "★",
                        Foreground = i < rating ? yellowBrush : whiteBrush
                    };
                    stackPanel.Children.Add(star);
                }

                return stackPanel;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

