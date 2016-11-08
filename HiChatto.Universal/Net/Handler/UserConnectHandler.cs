using HiChatto.Base.Net;
using HiChatto.Models;
using HiChatto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.Net.Handler
{
    [PackageHandler(ePackageType.USER_CONNECT, "User Connected Handler")]
    public class UserConnectHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            MainViewModel vm = sender as MainViewModel;
            if (vm == null)
            {
                return false;
            }
            UserInfo info = pkg.ReadObject<UserInfo>();
            vm.User = info;
            return true;
        }
    }
}
