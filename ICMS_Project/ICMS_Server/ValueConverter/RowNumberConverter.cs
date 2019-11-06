using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ICMS_Server
{
    public class RowNumberConverter : BaseValueConverter<RowNumberConverter>
    {
        static RowNumberConverter converter;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridRow row = value as DataGridRow;
            if (row != null)
                return row.GetIndex();
            else
                return -1;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (converter == null) converter = new RowNumberConverter();
            return converter;
        }

        public RowNumberConverter()
        {
        }
    }
}
