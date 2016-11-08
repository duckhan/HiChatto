using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Models
{
    [DataContract(Namespace = "HiChatto.Models")]
    public class Message
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
        public eMessageType Type { get; set; }
        public bool IsReceived { get; set; }
        public Message()
        {
            IsReceived = false;
            Type = eMessageType.Text;
        }
        public Message(eMessageType type)
        {
            IsReceived = false;
            this.Type = type;
        }
    }
}
