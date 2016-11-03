using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Base.Net
{
    public interface IPackageHandler
    {
        bool Handle(object sender, Package pkg);
    }
}
