using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HiChatto.Universal.Models
{
    public class UserInfo
    {
        public ulong UserID { get; set; }
        public string Name { get; set; }
        public string DeviceName { get; set; }
        public string CurrentIP { get; set; }
    }
}
