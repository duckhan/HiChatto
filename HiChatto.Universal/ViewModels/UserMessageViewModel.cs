using GalaSoft.MvvmLight;
using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.ViewModels
{
    public class UserMessageViewModel:BaseViewModel
    {
        private UserInfo _user;
        public UserInfo User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnProtertyChanged("User");
            }
        }
        public GroupMessageViewModel GroupMessage { get; set; }
        private string _CurentMessage;
        public string CurrentMessage
        {
            get { return _CurentMessage; }
            set
            {
                _CurentMessage = value;
                OnProtertyChanged("CurrentMessage");
            }
        }
        public UserMessageViewModel()
        {
            GroupMessage = new GroupMessageViewModel();
        }
    }
}
