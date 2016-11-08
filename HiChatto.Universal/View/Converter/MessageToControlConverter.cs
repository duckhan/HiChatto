using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace HiChatto.Universal.View.Converter
{
    public class MessageToControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Message info = value as Message;
            if (info == null)
            {
                return null;
            }
            Border g = new Border();          
            switch (info.Type)
            {
                case eMessageType.Text:
                    TextBlock t = new TextBlock();
                    g.Style = info.IsReceived ? Application.Current.Resources["BorderRecieveText"] as Style : Application.Current.Resources["BorderSentText"] as Style;
                    t.Text = info.Content == null ? "" : info.Content;
                    t.Padding = new Thickness(5);
                    g.Child = t;
                    return g;
                case eMessageType.Image:
                    Image img = new Image();
                    img.Style = Application.Current.Resources["ImageMessageStyle"] as Style;
                    img.Source = new BitmapImage(new Uri(info.Content));
                    g.Child = img;
                    return g;
                case eMessageType.Stick:
                    break;
                default:
                    break;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
