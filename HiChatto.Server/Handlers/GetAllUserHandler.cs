using HiChatto.Base.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Server.Handlers
{
    [PackageHandler(ePackageType.GET_ALL_USER,"Get all user handler")]
    public class GetAllUserHandler : IPackageHandler
    {
        public bool Handle(object sender, Package pkg)
        {
            Client c = sender as Client;
            if (c == null)
            {
                return false;
            }
            Package packet = new Package(ePackageType.GET_ALL_USER);
            packet.WriteInt(Server.Clients.Count - 1);
            foreach (Client client in Server.Clients)
            {
                if (client == null || client.ID == c.ID)
                {
                    continue;
                }
                packet.WriteObject(client.User);
            }
            c.Send(packet);
            return true;
        }
    }
}
