using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Base.Net
{
    public interface IPackageHandler
    {
        int Handle(NetSource source, Package pkg);
    }
}
