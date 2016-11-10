using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Models
{
    public class UserMessage : NotifyModel
    {

        private string _CurrentContent;
        public string CurrentContent
        {
            get { return _CurrentContent; }
            set
            {
                _CurrentContent = value;
                OnPropertyChanged("CurrentContent");
            }
        }
        private MessageCollection _Messages;
        public MessageCollection Messages
        {
            get { return _Messages; }
        }
        private UserInfo _user;
        public UserInfo User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("Users");
            }
        }
        private int _unReadCount;
        public int UnReadCount
        {
            get { return _unReadCount; }
            set
            {
                _unReadCount = value;
                OnPropertyChanged("UnReadCount");
            }
        }
        public void AddMessage(Message message)
        {
            _Messages.Add(message);
            OnPropertyChanged("Messages");
        }

        public UserMessage()
        {
            _user = null;
            _Messages = new MessageCollection();
        }
        public UserMessage(UserInfo user)
        {
            _user = user;
            _Messages = new MessageCollection();
        }
    }
}
