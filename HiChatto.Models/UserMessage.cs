using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HiChatto.Models
{
    public class UserMessage
    {
        public UserInfo User { get; set; }
        public string Name { get { return User.UserName; } }
        public MessageGroup Messages { get; set; }
        public UserMessage()
        {
            Messages = new MessageGroup();
        }
    }
}
