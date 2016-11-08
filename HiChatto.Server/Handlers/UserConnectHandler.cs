using HiChatto.Base.Net;
using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Server.Handlers
{
    [PackageHandler((int)ePackageType.USER_CONNECT)]
    public class UserConnectHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            Client c = (Client)sender;
            UserInfo user = pkg.ReadObject<UserInfo>();
            user.UserID = Statistics.NewUserID;
            c.User = user;
            pkg = new Package(pkg.Code);
            pkg.WriteObject(user);
            c.Send(pkg);//Send userid to client

            //Send User Online to All Clients
            pkg = new Package(ePackageType.USER_ONLINE);
            pkg.WriteObject(user);
            foreach (Client item in Server.Clients)
            {
                if (item.ID != user.UserID)
                {
                    item.Send(pkg);
                }
            }
            return true;
        }
    }
}
