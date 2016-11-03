using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;

namespace HiChatto.Universal.ViewModels.Converter
{
    public class MessageToRichTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Type t = value.GetType();
            MessageGroup obj = value as MessageGroup;
            if (obj == null)
            {
                return null;
            }
            RichTextBlock r = new RichTextBlock();
            foreach (MessageInfo item in obj)
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run() { Text = item.Content });
                r.Blocks.Add(p);
            }
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
