using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Models
{
    /// <summary>
    /// A list of MessageInfo
    /// </summary>
    public class MessageGroup : List<MessageInfo>
    {
        public int GroupID { get; set; }

        /// <summary>
        /// list Use in this conversation
        /// </summary>
        public ObservableCollection<UserInfo> Users { get; set; }
        public MessageGroup()
        {
            Users = new ObservableCollection<UserInfo>();
        }
    }
}
