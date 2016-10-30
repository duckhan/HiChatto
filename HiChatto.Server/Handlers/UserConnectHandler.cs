using HiChatto.Base.Net;
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
        public int Handle(NetSource source, Package pkg)
        {
            Client c = (Client)source;
            Console.WriteLine("Process package code :{0}", pkg.Code);
            return 0;
        }
    }
}
