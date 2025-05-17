﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DeltaBrainsJSCAppFE.Handel
{
    public class BoolToEditTitleConverter : IValueConverter
    {
        //Thay đổi title trong window task giữa thêm và sửa
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "CHỈNH SỬA CÔNG VIỆC" : "THÊM CÔNG VIỆC MỚI";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

}
