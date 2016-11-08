using HiChatto.Base.Net;
using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.Net
{
    public class PackageOut:IPackageOut
    {
        UniversalClient client;
        public PackageOut(UniversalClient c)
        {
            client = c;
        }

        /// <summary>
        /// Send User info to Server and recieve UserID
        /// </summary>
        public void SendUserConnect()
        {
            Package pkg = new Package(ePackageType.USER_CONNECT);
            pkg.WriteObject(client.User);
            client.Send(pkg);
        }

        public void SendTextMessage(Message mess)
        {
            Package pkg = new Package(ePackageType.TEXT_MESSAGE);
            pkg.WriteObject(mess);
            client.Send(pkg);
        }
        public void SendGetAllUser()
        {
            Package pkg = new Package(ePackageType.GET_ALL_USER);
            client.Send(pkg);
        }
    }
}
