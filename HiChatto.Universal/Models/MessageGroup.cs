using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.Models
{
    public class MessageGroup
    {
        public ulong GroupID { get; set; }
        public List<MessageInfo> Messages { get; set; }
        public MessageGroup()
        {
            Messages = new List<MessageInfo>();
        }
    }
}
