using System;
using System.Linq;
using Cirrious.CrossCore.Converters;

namespace TestTutorial.Converters
{
    public class StringFormatValueConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format((string)parameter, value );
        }
    }
}
