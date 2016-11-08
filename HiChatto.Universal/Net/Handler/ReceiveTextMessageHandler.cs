﻿using HiChatto.Base.Net;
using HiChatto.Models;
using HiChatto.ViewModels;

namespace HiChatto.Universal.Net.Handler
{
    [PackageHandler((int)ePackageType.TEXT_MESSAGE,"Receive text message handler")]
    public class ReceiveTextMessageHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            if (sender is MainViewModel)
            {
                pkg.ResetOffset();
                Message obj=pkg.ReadObject<Message>();
                if (!(obj is Message))
                {
                    return false;
                }
                MainViewModel vm = sender as MainViewModel;
                Message mess = obj as Message;
                mess.IsReceived = true;
                vm.AddMessageInfo(mess);
                return true;
            }
            return false;
        }
    }
}
