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
using GalaSoft.MvvmLight.Ioc;
using HiChatto.Universal.ViewModels.Navigation;
using GalaSoft.MvvmLight.Views;

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
        INavigationService navigationService;
        public StartViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            _config = SimpleIoc.Default.GetInstance<ClientConfig>();
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
                    ClientConfig config = JsonConvert.DeserializeObject<ClientConfig>(str);
                    if (config != null)
                    {
                        _config.ServerIP = config.ServerIP;
                        _config.ServerPort = config.ServerPort;
                        _config.UserName = config.UserName;
                    }
                }
            }
            catch
            {
               
            }
        }
        private void Connect()
        {
            Client c = SimpleIoc.Default.GetInstance<Client>();
            if (c == null)
            {
                return;
            }

            c.Connect();
            c.Connected += Client_Connected;
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            if ((sender as Client).IsConnected)
            {
                UserInfo u = new UserInfo() { UserID = 1, UserName = "DucKhan" };
                MessageInfo info = new MessageInfo();
                    info.Content = "DUCKHAn adaksaljs";
                info.ID = 1;
                info.GroupID = 2;
                Package pkg = new Package(ePackageType.TEXT_MESSAGE);
                pkg.WriteObject(info, typeof(MessageInfo));
                (sender as Client).Send(pkg);
                navigationService.NavigateTo("MainView");
            }
            else
            {
                
            }
        }
    }
}
