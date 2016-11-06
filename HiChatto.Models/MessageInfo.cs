using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Models
{
    [DataContract(Namespace = "HiChatto.Models")]
    public class MessageInfo
    {
        [DataMember]
        public int IDSender { get; set; }
        [DataMember]
        public int IDReceiver { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime ReceiveTime { get; set; }
        [DataMember]
        public int GroupID { get; set; }

        public bool IsReceived { get; set; }
        public MessageInfo()
        {

        }
    }
}
