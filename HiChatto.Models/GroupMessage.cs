using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Models
{
    public class GroupMessage : NotifyModel
    {
        private int _GroupID;
        public int GroupID
        {
            get { return _GroupID; }
            set
            {
                _GroupID = value;
                OnPropertyChanged("GroupID");
            }
        }

        private string _CurrentContent;
        public string CurrentContent
        {
            get { return _CurrentContent; }
            set
            {
                if (_CurrentContent.Equals(value))
                {
                    return;
                }
                _CurrentContent = value;
                OnPropertyChanged("Current");
            }
        }

        private ObservableCollection<MessageInfo> _Messages;
        public ObservableCollection<MessageInfo> Messages
        {
            get { return _Messages; }
        }
        public void AddMessage(MessageInfo message)
        {
            _Messages.Add(message);
            OnPropertyChanged("Messages");
        }

        public GroupMessage()
        {
            _Messages = new ObservableCollection<MessageInfo>();
        }
    }
}
