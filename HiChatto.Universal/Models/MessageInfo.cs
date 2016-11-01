using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.Models
{
    public class MessageInfo
    {
        public ulong ID { get; set; }
        public string Content { get; set; }
        public DateTime ReceiveTime { get; set; }
        public bool IsRead { get; set; }
    }
}
