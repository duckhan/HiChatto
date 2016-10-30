using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using HiChatto.Base.Config;
using System.Reflection;

namespace HiChatto.Server
{
    public class ServerConfig : BaseAppConfig
    {
        [ConfigProperty("IP", "127.0.0.1")]
        public string IP;
        [ConfigProperty("Port", 9696)]
        public int Port;

        public ServerConfig()
        {
            Load(typeof(ServerConfig));
        }

        protected override void Load(Type type)
        {
            foreach (FieldInfo f in type.GetFields())
            {
                ConfigPropertyAttribute[] attrs = (ConfigPropertyAttribute[])f.GetCustomAttributes(typeof(ConfigPropertyAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }
                f.SetValue(this, LoadPropertyAttribute(attrs[0], ConfigurationManager.AppSettings[attrs[0].Key]));
            }
        }
    }
}
