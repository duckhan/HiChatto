using System;
using System.Reflection;
using System.IO;
namespace HiChatto.Models
{
    public class ClientConfig
    {   
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public string UserName { get; set; }
        public ClientConfig()
        {
            ServerPort = 6969;
        }
    }
}
