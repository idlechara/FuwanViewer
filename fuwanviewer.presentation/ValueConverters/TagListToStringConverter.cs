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
    [ValueConversion(typeof(List<Tag>), typeof(string))]
    public class TagListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable<Tag> tags = value as IEnumerable<Tag> ?? new List<Tag>();

            if (tags == null || tags.Count() == 0)
                return string.Empty;

            StringBuilder sbResult = new StringBuilder();
            string separator = ", ";

            foreach (var tag in tags)
            {
                sbResult.Append(tag.ToString()).Append(separator);
            }
            sbResult.Length = sbResult.Length - separator.Length;

            return sbResult.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
