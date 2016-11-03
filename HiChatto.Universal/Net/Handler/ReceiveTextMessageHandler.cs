using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiChatto.Base.Net;
using HiChatto.Universal.ViewModels;
using HiChatto.Models;

namespace HiChatto.Universal.Net.Handler
{
    [PackageHandler((int)ePackageType.TEXT_MESSAGE,"Receive text message handler")]
    class ReceiveTextMessageHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            if (sender is MainViewModel)
            {
                pkg.ResetOffset();
                object obj=pkg.ReadObject(typeof(MessageInfo));
                if (!(obj is MessageInfo))
                {
                    return false;
                }
                MainViewModel vm = sender as MainViewModel;
                MessageInfo mess = obj as MessageInfo;
                UserMessage g = vm.UserMessages.SingleOrDefault(u => u.Messages.GroupID == mess.GroupID);
                g.Messages.Add(mess);
                return true;
            }
            return false;
        }
    }
}
