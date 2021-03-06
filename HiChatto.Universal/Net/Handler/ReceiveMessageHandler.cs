﻿using HiChatto.Base.Net;
using HiChatto.Models;
using HiChatto.ViewModels;

namespace HiChatto.Universal.Net.Handler
{
    [PackageHandler((int)ePackageType.USER_MESSAGE, "Receive text message handler")]
    public class ReceiveMessageHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            try
            {
                if (sender is MainViewModel)
                {
                    pkg.ResetOffset();
                    Message obj = pkg.ReadObject<Message>();
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
            }
            catch (System.Exception)
            {
            }
            return false;
        }
    }
}
