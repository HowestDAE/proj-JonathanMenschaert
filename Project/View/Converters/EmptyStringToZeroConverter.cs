using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Project.View.Converters
{
    public class EmptyStringToZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Returns "0" if the string is empty or null, otherwise the Damage value is not displayed
            string strVal = value.ToString();
            if (string.IsNullOrEmpty(strVal))
            {
                return "0";
            }
            else
            {
                return strVal;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
