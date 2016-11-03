﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiChatto.Base.Net;
using HiChatto.Models;

namespace HiChatto.Server.Handlers
{
    [PackageHandler((int)ePackageType.TEXT_MESSAGE, "Text message handler")]
    public class UserSendTextMessagesHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            try
            {
                MessageInfo mess = (MessageInfo)pkg.ReadObject(typeof(MessageInfo));
                Console.WriteLine("Message conent: {0}", mess.Content);
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }
    }
}
