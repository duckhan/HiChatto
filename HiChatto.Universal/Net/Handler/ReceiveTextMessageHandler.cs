﻿using System;
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
    public class ReceiveTextMessageHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            if (sender is MainViewModel)
            {
                pkg.ResetOffset();
                MessageInfo obj=pkg.ReadObject<MessageInfo>();
                if (!(obj is MessageInfo))
                {
                    return false;
                }
                MainViewModel vm = sender as MainViewModel;
                MessageInfo mess = obj as MessageInfo;
                mess.IsReceived = true;
                vm.AddMessageInfo(mess);
                return true;
            }
            return false;
        }
    }
}
