using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.ViewModels
{
    public class GroupMessageViewModel : BaseViewModel
    {
        private int _GroupID;
        public int GroupID
        {
            get { return _GroupID; }
            set
            {
                _GroupID = value;
                OnProtertyChanged("GroupID");
            }
        }
        private List<MessageInfo> _Messages;
        public List<MessageInfo> Messages
        {
            get { return _Messages; }
        }
        public void AddMessage(MessageInfo message)
        {
            _Messages.Add(message);
            OnProtertyChanged("Messages");
        }

        public GroupMessageViewModel()
        {
            _Messages = new List<MessageInfo>();
        }
    }
}
