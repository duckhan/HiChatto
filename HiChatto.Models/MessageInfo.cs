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
        public int GroupID { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime ReceiveTime { get; set; }

        [DataMember]
        public eMessageType Type { get; set; }

        public bool IsReceived { get; set; }
        public MessageInfo()
        {
            IsReceived = false;
            Type = eMessageType.Text;
        }
        public MessageInfo(eMessageType type)
        {
            IsReceived = false;
            this.Type = type;
        }
    }
}
