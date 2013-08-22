using System;
using System.Linq;
using Cirrious.CrossCore.Converters;

namespace TestTutorial.Converters
{
    public class InverseBooleanValueConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
             if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }
    }
}
