using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
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
            if (info.IsReceived)
            {
                g.SetBinding(Grid.MaxWidthProperty, new Binding() { ElementName = "LeftReferenceBorder", Path = new PropertyPath("ActualWidth") });
                if (info.Type == eMessageType.Text)
                {
                    g.Style = Application.Current.Resources["BorderRecieveText"] as Style;
                }             
            }
            else
            {
                g.SetBinding(Grid.MaxWidthProperty, new Binding() { ElementName = "RightReferenceBorder", Path = new PropertyPath("ActualWidth") });

                if (info.Type == eMessageType.Text)
                {
                    g.Style = Application.Current.Resources["BorderSentText"] as Style;
                }
            }
            switch (info.Type)
            {
                case eMessageType.Text:
                    TextBlock t = new TextBlock();
                    t.IsTextSelectionEnabled = true;
                    t.Text = info.Content == null ? "" : info.Content;
                    t.Padding = new Thickness(5);
                    t.TextWrapping = TextWrapping.Wrap;
                    g.Child = t;
                    return g;
                case eMessageType.Image:
                    {
                        Image img = new Image();
                        img.Style = Application.Current.Resources["ImageMessageStyle"] as Style;
                        img.Source = new BitmapImage(new Uri(info.Content));
                        g.Child = img;
                        return g;
                    }
                case eMessageType.Sticky:
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(info.Content));
                        img.Width = 64;
                        img.MaxWidth = 100;
                        g.Child = img;
                        return g;
                    }
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
