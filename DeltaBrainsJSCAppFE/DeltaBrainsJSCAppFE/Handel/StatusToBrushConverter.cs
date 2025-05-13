using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DeltaBrainsJSCAppFE.Handel
{
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Brushes.Gray;

            string status = value.ToString().ToLower();
            switch (status)
            {
                case "hoàn thành":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
                case "đang thực hiện":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
                case "chưa thực hiện":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107"));
                default:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
