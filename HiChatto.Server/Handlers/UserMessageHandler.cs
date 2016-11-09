using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiChatto.Base.Net;
using HiChatto.Models;

namespace HiChatto.Server.Handlers
{
    [PackageHandler((int)ePackageType.USER_MESSAGE, "user message handler")]
    public class UserSendTextMessagesHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            try
            {
                Message mess = pkg.ReadObject<Message>();
                Client c = Server.GetClient(mess.IDReceiver);
                if (c == null)
                {
                    return false;
                }
                pkg.ResetOffset();
                c.Send(pkg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }
    }
}
