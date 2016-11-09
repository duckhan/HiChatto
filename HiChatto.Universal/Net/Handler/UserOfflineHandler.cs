using HiChatto.Base.Net;
using HiChatto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.Net.Handler
{
    [PackageHandler(ePackageType.USER_OFFINE, "User Offline Handler")]
    public class UserOfflineHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            MainViewModel vm = sender as MainViewModel;
            if (vm == null)
            {
                return false;
            }

            int userId = pkg.ReadInt();
            try
            {
                var u = vm.UserMessages.Single(q => q.User.UserID == userId);
                vm.UserMessages.Remove(u);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
    }
}
