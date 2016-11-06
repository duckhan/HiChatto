using HiChatto.Base.Net;
using HiChatto.Models;
using HiChatto.Universal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.Net.Handler
{
    [PackageHandler(ePackageType.GET_ALL_USER, "Get all user handler")]
    public class GetAllUserHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            MainViewModel vm = sender as MainViewModel;
            if (vm == null)
            {
                return false;
            }
            int len = pkg.ReadInt();
            for (int i = 0; i < len; i++)
            {
                UserInfo user = pkg.ReadObject<UserInfo>();
                if (user == null)
                {
                    continue;
                }
                vm.AddUser(user);
            }
            return true;
        }
    }
}
