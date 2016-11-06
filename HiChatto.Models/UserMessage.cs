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
        public MessageGroup Messages { get; set; }
        public MessageInfo Current { get; set; }
        public UserMessage()
        {
            Messages = new MessageGroup();
            Current = new MessageInfo();
        }
    }
}
