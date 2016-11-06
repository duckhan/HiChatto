using System.Runtime.Serialization;
namespace HiChatto.Models
{
    [DataContract(Namespace = "HiChatto.Models")]
    public class UserInfo
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string UserName { get; set; }
    }
}
