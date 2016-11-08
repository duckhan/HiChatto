using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace HiChatto.Universal.View.Converter
{
    public class UserCollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            UserCollection users = value as UserCollection;
            if (users == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < users.Count; i++)
            {
                builder.Append(users[i].UserName);
                if (i < users.Count - 1)
                {
                    builder.Append(", ");
                }
            }
            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
