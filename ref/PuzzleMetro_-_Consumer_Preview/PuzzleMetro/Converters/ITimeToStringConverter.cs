using System;
using Windows.UI.Xaml.Data;

namespace Puzzle15.Converters
{
    public class ITimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var time = (DateTime)value;
            return string.Format("{0}", time.ToString("HH:mm:ss"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
