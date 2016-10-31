using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiChatto.Base.Config;
using Windows.Storage;
using System.IO.IsolatedStorage;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;

namespace HiChatto.Universal
{
    public class UniversalConfig : BaseAppConfig
    {   
        public string ServerIP { get; set; }
        [ConfigProperty("ServerPort",6969)]
        public string ServerPort { get; set; }
        public string UserName { get; set; }
        protected async override void Load(Type type)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile settingFile = await localFolder.GetFileAsync("setting.json");
            if (settingFile == null)
            {
                LoadDefaultProperties(type);
            }
            else
            {
                string data = File.ReadAllText(settingFile.Path);
                UniversalConfig config = JsonConvert.DeserializeObject<UniversalConfig>(data);
                ServerIP = config.ServerIP;
                ServerPort = config.ServerPort;
                UserName = config.UserName;
            }
        }
        private void LoadDefaultProperties(Type type)
        {
            foreach (var item in type.GetFields())
            {
                ConfigPropertyAttribute[] attr = (ConfigPropertyAttribute[])item.GetCustomAttributes(typeof(ConfigPropertyAttribute),false);
                if (attr.Length>0)
                {
                    item.SetValue(attr[0].Key, attr[0].Value);
                }
            }
        }
    }
}
