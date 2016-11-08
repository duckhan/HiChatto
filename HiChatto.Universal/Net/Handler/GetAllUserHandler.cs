using HiChatto.Base.Net;
using HiChatto.Models;
using HiChatto.ViewModels;

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
            int count = pkg.ReadInt();
            for (int i = 0; i < count; i++)
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
