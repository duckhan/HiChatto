using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;

namespace HiChatto.Server
{
    public class ServerConfig
    {
        public string IP;
        public int Port;

        public ServerConfig()
        {
            Load();
        }

        protected void Load()
        {
            IP = ConfigurationManager.AppSettings["IP"];
            Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
        }
    }
}
