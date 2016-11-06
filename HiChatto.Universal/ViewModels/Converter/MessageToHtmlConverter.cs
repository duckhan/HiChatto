using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;

namespace HiChatto.Universal.ViewModels.Converter
{
    public class MessageToHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Type t = value.GetType();
            List<MessageInfo> obj = value as List<MessageInfo>;
            if (obj == null)
            {
                return null;
            }
            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<div width=\"100%\">");
            foreach (MessageInfo item in obj)
            {
                string p = string.Format("<div style=\"background:{0};text-align:{1}\">{2}</div>", item.IsReceived ? "#3498db" : "#2ecc71", item.IsReceived ? "left" : "right", item.Content);
                htmlBuilder.Append(p);
            }
            htmlBuilder.Append("</div>");
            return htmlBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
