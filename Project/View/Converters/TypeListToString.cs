using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Project.View.Converters
{
    public class TypeListToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> typeList = (value as List<string>);
            if (typeList == null)
            {
                return null;
            }
            Dictionary<string, int> typeMapping = new Dictionary<string, int>();

            foreach(string type in typeList)
            {
                if (typeMapping.ContainsKey(type))
                {
                    ++typeMapping[type];
                }
                else
                {
                    typeMapping.Add(type, 1);
                }
            }

            string typeOutput = "";

            foreach(string key in typeMapping.Keys)
            {
                if (!string.IsNullOrEmpty(typeOutput))
                {
                    typeOutput += ", ";
                }
                typeOutput += $"{typeMapping[key]}x {key}";
            }
            return typeOutput;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
