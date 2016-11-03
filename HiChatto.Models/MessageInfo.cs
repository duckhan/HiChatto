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
        public int ID { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime ReceiveTime { get; set; }
        [DataMember]
        public bool IsReceived { get; set; }
        [DataMember]
        public int GroupID { get; set; }
    }
}
