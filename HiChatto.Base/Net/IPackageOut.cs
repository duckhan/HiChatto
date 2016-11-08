using HiChatto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Base.Net
{
    public interface IPackageOut
    {
        void SendUserConnect();
        void SendTextMessage(Message mess);
        void SendGetAllUser();
    }
}
