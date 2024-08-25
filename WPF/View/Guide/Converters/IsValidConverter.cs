using Emoji.Wpf;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TextBlock = Emoji.Wpf.TextBlock;

namespace BookingApp.WPF.View.Guide.Converters
{

  
        public class IsValidConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is string isValidString)
                {
                    bool isValid = bool.TryParse(isValidString, out bool result);

                    string symbol = result ? "✔️" : "❌";
                    Brush color = result ? Brushes.Green : Brushes.Red;

                    TextBlock textBlock = new TextBlock
                    {
                        Text = symbol,
                        Foreground = color
                    };

                    return textBlock;
                }
                return string.Empty;
            }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }



