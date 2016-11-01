using HiChatto.Base.Net;
using HiChatto.ClientLib;
using HiChatto.ClientLib.Models;
using HiChatto.ClientLib.Net;
using HiChatto.ClientLib.ViewModels;
using System;
using System.Windows.Input;

namespace HiChatto.ClientLib.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        ClientConfig _config;
        public ClientConfig Config
        {
            get { return _config; }
            set
            {
                if (value != _config)
                {
                    _config = value;
                    OnPropertyChanged("Config");
                }
            }
        }
        NetSource _client;
        private bool _IsConnectable = false;
        public bool IsConnectable
        {
            get { return _IsConnectable; }
            set
            {
                if (value != _IsConnectable)
                {
                    _IsConnectable = value;
                    OnPropertyChanged("IsConnectable");
                }
            }
        }
        public StartViewModel(string s)
        {
            _config = new ClientConfig();
            IsConnectable = _config.ServerIP != null && _config.UserName!=null;
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

        }
        private void Connect()
        {
            _client = new Client(new byte[8096], new byte[8096]);
            //_client.Connect(_config);
            _client.Connected += Client_Connected;
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
