using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TaskClientWPF.Converters
{
    public class TagToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                switch (value)
                {
                    case "Green":
                        return Brushes.YellowGreen;
                    case "Red":
                        return Brushes.Tomato;
                    case "Yellow":
                        return Brushes.Yellow;
                    default:
                        return Brushes.Transparent;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
