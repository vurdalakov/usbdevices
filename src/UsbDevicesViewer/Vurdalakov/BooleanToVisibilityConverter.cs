namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Boolean Reverse { get; set; }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (System.Convert.ToBoolean(value) ^ this.Reverse) ? Visibility.Visible : Visibility.Collapsed;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value.Equals(Visibility.Visible) ^ this.Reverse;
        }
    }
}
