﻿using Lab2TaskClient;
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
            if(value is Tag tag)
            {
                switch (tag.TagColour)
                {
                    case "Green":
                        return Brushes.Green;
                    case "Red":
                        return Brushes.Red;
                    case "Yellow":
                        return Brushes.Yellow;
                    default:
                        return Brushes.Transparent;
                }
            } 
            else 
                return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
