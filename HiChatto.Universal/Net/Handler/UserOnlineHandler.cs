using HiChatto.Base.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiChatto.Models;
using HiChatto.ViewModels;

namespace HiChatto.Universal.Net.Handler
{
    [PackageHandler((int)ePackageType.USER_ONLINE,"User Online Handler")]
    public class UserOnlineHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            MainViewModel vm = sender as MainViewModel;
            if (vm == null)
            {
                return false;
            }
            UserInfo user=pkg.ReadObject<UserInfo>();
            vm.AddUser(user,true);
            return true;
        }
    }
}
