using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Models
{
    public class MessageGroup:List<MessageInfo>
    {
        public int GroupID { get; set; }
        public MessageGroup()
        {
        }
    }
}
