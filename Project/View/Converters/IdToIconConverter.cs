using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Project.View.Converters
{
    public class IdToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Convert the pokemon id to the symbol image url
            string id = value.ToString();
            return new Uri($"https://images.pokemontcg.io/{id.Substring(0, id.IndexOf("-"))}/symbol.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
