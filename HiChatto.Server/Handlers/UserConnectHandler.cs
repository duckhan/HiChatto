using HiChatto.Base.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Server.Handlers
{
    [PackageHandler((int)ePackageType.TEXT_MESSAGE)]
    public class UserConnectHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            Client c = (Client)sender;
            Console.WriteLine("Process package code :{0}", pkg.Code);
            return true;
        }
    }
}
