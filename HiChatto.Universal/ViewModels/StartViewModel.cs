using HiChatto.Models;
using HiChatto.Universal.Net;
using System;
using System.IO;
using System.Windows.Input;
using Windows.Storage;
using Newtonsoft.Json;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using HiChatto.Universal.ViewModels.Communicate;
using GalaSoft.MvvmLight.Command;

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
        IMessagerSercive messageService;
        public StartViewModel(IMessagerSercive navigationService)
        {
            this.messageService = navigationService;
            _config = SimpleIoc.Default.GetInstance<ClientConfig>();
            LoadConfigAsync();
            IsConnectable = _config != null && _config.ServerIP != null && _config.UserName != null;
        }
        public RelayCommand SaveConfigCommand
        {
            get { return new RelayCommand(SaveConfig); }
        }
        public ICommand ConnectCommand
        {
            get { return new RelayCommand(Connect); }
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
            UniversalClient c = SimpleIoc.Default.GetInstance<UniversalClient>();
            if (c == null)
            {
                return;
            }

            c.Connect();
            c.Connected += Client_Connected;
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            if ((sender as UniversalClient).IsConnected)
            {
                messageService.NavigateTo("MainView");
            }
            else
            {
                messageService.ShowMessage("Can't connect. Try again", "Error");
            }
        }
    }
}
