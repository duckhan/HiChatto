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
        public int ServerPort { get; set; }
        public string UserName { get; set; }
        public UniversalConfig()
        {
            Load(typeof(UniversalConfig));
        }
        protected async override void Load(Type type)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            { 
                StorageFile settingFile = await localFolder.GetFileAsync("setting.json");
                string data = File.ReadAllText(settingFile.Path);
                UniversalConfig config = JsonConvert.DeserializeObject<UniversalConfig>(data);
                ServerIP = config.ServerIP;
                ServerPort = config.ServerPort;
                UserName = config.UserName;
            }
            catch
            {
                LoadDefaultProperties(type);
            }
        }
        private void LoadDefaultProperties(Type type)
        {
            foreach (var item in type.GetProperties())
            {
                ConfigPropertyAttribute[] attr = (ConfigPropertyAttribute[])item.GetCustomAttributes(typeof(ConfigPropertyAttribute),false);
                if (attr.Length>0)
                {
                    item.SetValue(this, attr[0].Value);
                }
            }
        }
        public async void Save()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile settingFile = await localFolder.CreateFileAsync("setting.json",CreationCollisionOption.ReplaceExisting);
            string data = JsonConvert.SerializeObject(this);
            File.WriteAllText(settingFile.Path, data);
        }
    }
}
