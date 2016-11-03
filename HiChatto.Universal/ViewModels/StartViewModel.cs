using HiChatto.Base.Net;
using HiChatto.Models;
using HiChatto.Universal.Net;
using HiChatto.Universal.ViewModels.Command;
using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Newtonsoft.Json;
using GalaSoft.MvvmLight;

namespace HiChatto.Universal.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        ClientConfig _config;
        public ClientConfig Config
        {
            get { return _config; }
            set
            {
                Set("Config", ref _config, value);
            }
        }
        private bool _IsConnectable = false;
        public bool IsConnectable
        {
            get { return _IsConnectable; }
            set
            {
                Set("IsConnectable", ref _IsConnectable, value);
            }
        }

        public StartViewModel()
        {
            LoadConfigAsync();
            IsConnectable = _config != null && _config.ServerIP != null && _config.UserName != null;
        }
        public ICommand SaveConfigCommand
        {
            get { return new DelegateCommand(SaveConfig); }
        }
        public ICommand ConnectCommand
        {
            get { return new DelegateCommand(Connect); }
        }
        private void SaveConfig()
        {
            IsConnectable = _config?.ServerIP != null && _config?.UserName != null;
            SaveConfigAsync();

        }
        private async void SaveConfigAsync()
        {
            var local = ApplicationData.Current.LocalFolder;
            var file =await local.CreateFileAsync("setting.json", CreationCollisionOption.ReplaceExisting);         
            string str = JsonConvert.SerializeObject(_config);

            File.WriteAllText(file.Path, str);
        }
        private async void LoadConfigAsync()
        {
            try
            {
                var local = ApplicationData.Current.LocalFolder; ;
                var file = await local.GetFileAsync("setting.json");
                if (file.IsAvailable)
                {
                    string str = File.ReadAllText(file.Path);
                    _config = JsonConvert.DeserializeObject<ClientConfig>(str);
                    _config = _config == null ? new ClientConfig() : _config;
                }
            }
            catch
            {
                _config = new ClientConfig();
            }
        }
        private void Connect()
        {
            Client c = new Client(_config);
            c.Connect();
            MessageInfo s = new MessageInfo();
            s.Content = "DucKhan";
            s.GroupID = 1;
            s.ID = 1;
            s.IsReceived = false;
            Package pkg = new Package((int)ePackageType.TEXT_MESSAGE);
            pkg.WriteObject(s, typeof(MessageInfo));
            c.Send(pkg);
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            CoreApplication.Properties.Add("Client", sender);
        }
    }
}
