namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class IntegerToObjectConverter : IValueConverter
    {
        public Object DefaultValue { get; set; }
        public Object Value0 { get; set; }
        public Object Value1 { get; set; }
        public Object Value2 { get; set; }
        public Object Value3 { get; set; }
        public Object Value4 { get; set; }

        public object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            switch (System.Convert.ToInt32(value))
            {
                case 0:
                    return this.Value0;
                case 1:
                    return this.Value1;
                case 2:
                    return this.Value2;
                case 3:
                    return this.Value3;
                case 4:
                    return this.Value4;
                default:
                    return this.DefaultValue;
            }
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
