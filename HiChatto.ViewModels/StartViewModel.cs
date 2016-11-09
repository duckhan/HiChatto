using HiChatto.Models;
using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using HiChatto.ViewModels.Communicate;
using GalaSoft.MvvmLight.Command;
using HiChatto.Base.Net;

namespace HiChatto.ViewModels
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
        NetSource client;
        public StartViewModel(IMessagerSercive navigationService,NetSource client)
        {
            this.client = client;
            this.messageService = navigationService;
            Config = client.Config;
            //LoadConfigAsync();
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
            Action<ClientConfig> save = SimpleIoc.Default.GetInstance<Action<ClientConfig>>();
            save(_config);

        }

        private void Connect()
        {
            client.Connect(_config);
            client.Connected += Client_Connected;
        }

        private void Client_Connected(object sender, EventArgs e)
        {

            if ((sender as NetSource).IsConnected)
            {
                messageService.NavigateTo("MainView",client);
            }
            else
            {
                messageService.ShowMessage("Can't connect. Try again", "Error");
            }
        }
    }
}
