using System;
using Windows.Storage;
using System.Reflection;
using System.IO;
namespace HiChatto.Universal.Models
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
