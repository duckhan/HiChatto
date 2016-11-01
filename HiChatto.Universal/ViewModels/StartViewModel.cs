using HiChatto.Base.ViewModels;
using System;
using System.Windows.Input;

namespace HiChatto.Universal.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        UniversalConfig _config;
        public UniversalConfig Config
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
        UniversalClient _client;
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
            _config = new UniversalConfig();
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
            _config?.Save();
        }
        private void Connect()
        {
            _client = new UniversalClient(new byte[8096], new byte[8096]);
            _client.Connect(_config);
            _client.Connected += Client_Connected;
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
