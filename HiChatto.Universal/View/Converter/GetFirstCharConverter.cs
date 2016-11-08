using System;
using Windows.UI.Xaml.Data;

namespace HiChatto.Universal.View.Converter
{
    public class GetFirstCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string s = value as string;
            return s?[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
