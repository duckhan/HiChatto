using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiChatto.Universal.Models;
namespace HiChatto.Universal.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        public Dictionary<ulong, UserMessage> UserMessages;
        public MainViewModel()
        {
            UserMessages = new Dictionary<ulong, UserMessage>();
        }
    }
}
