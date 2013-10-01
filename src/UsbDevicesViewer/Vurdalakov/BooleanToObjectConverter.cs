namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class BooleanToObjectConverter : IValueConverter
    {
        public Object TrueValue { get; set; }
        public Object FalseValue { get; set; }

        public object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? this.TrueValue : this.FalseValue;
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value.Equals(this.TrueValue);
        }
    }
}
