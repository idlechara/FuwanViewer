using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using FuwanViewer.Model.VisualNovels;

namespace FuwanViewer.Presentation.ValueConverters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BytesToStringConverter : IValueConverter
    {
        //Taken from http://www.java2s.com/Code/CSharp/Data-Types/ConvertlongvaluetoKBMBGBTB.htm
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] suffix = new string[] {"KB", "MB", "GB", "TB", "HB" };
            float byteNumber = (float)((int)value);
            for (int i = 0; i < suffix.Length; i++)
            {
                if (byteNumber < 1000)
                    if (i == 0)
                        return string.Format("{0} {1}", byteNumber, suffix[i]);
                    else
                        return string.Format("{0:0.#0} {1}", byteNumber, suffix[i]);
                else
                    byteNumber /= 1024;
            }
            return string.Format("{0:N} {1}", byteNumber, suffix[suffix.Length - 1]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
