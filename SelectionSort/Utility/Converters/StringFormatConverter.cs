using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace SelectionSort.Utility.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int input )
            {
                return input.ToString("D3");
            }
            //if (value == null) return string.Empty;

            //string formatString = parameter as string;
            //if (string.IsNullOrEmpty(formatString))
            //    return value.ToString();

            return "000";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
