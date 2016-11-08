﻿using HiChatto.Base.Net;
using HiChatto.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using HiChatto.Universal.Net;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using System;
using HiChatto.Universal.ViewModels.Communicate;
using GalaSoft.MvvmLight.Threading;

namespace HiChatto.Universal.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields/Properties

        private UserMessageCollection _UserMessages;
        public UserMessageCollection UserMessages
        {
            get { return _UserMessages; }
            set
            {
                Set("UserMessages", ref _UserMessages, value);
            }
        }
        private UserMessage _selected;
        public string CurrentContent
        {
            get
            {
                return _selected?.CurrentContent;
            }
            set
            {
                _selected.CurrentContent = value;
                RaisePropertyChanged("CurrentContent");
            }
        }


        private UserCollection _OnlineUsers;
        public UserCollection OnlineUsers
        {
            get
            {
                return _OnlineUsers;
            }
            set
            {
                Set("OnlineUsers", ref _OnlineUsers, value);
            }
        }

        public IPackageHandler[] Handler
        {
            get { return ((App)App.Current).Handlers; }
        }
        public UserInfo User
        {
            get { return client?.User; }
            set
            {
                client.User = value;
                RaisePropertyChanged("User");
            }
        }
        private bool _ContentVistable;
        public bool IsMessageContentVisitable
        {
            get { return _ContentVistable; }
            set
            {
                Set("IsMessageContentVisitable", ref _ContentVistable, value);
            }
        }
        private IMessagerSercive messagerService;
        private Client client;
        private PackagesOut Out;

        #endregion

        #region Contructor
        public MainViewModel(IMessagerSercive service)
        {

            _UserMessages = new UserMessageCollection();
#if DEBUG
            _UserMessages = GetSampleGroup();
#endif
            _OnlineUsers = new UserCollection();
            _ContentVistable = false;
            this.messagerService = service;
            client = SimpleIoc.Default.GetInstance<Client>();
            client.Received += Client_Received;
            client.RecieveAsync();
            Out = new PackagesOut(client);
            User.UserName = client.Config.UserName;
            Out.SendUserConnect();
            Task.Delay(2000).ContinueWith((task) =>
            {
                Out.SendGetAllUser();
            });
        }
        #endregion

        #region Command
        public RelayCommand<UserMessage> ListViewItemSelected { get { return new RelayCommand<UserMessage>(ItemSelectedHandle); } }
        private void ItemSelectedHandle(UserMessage g)
        {
            _selected = g;
            IsMessageContentVisitable = true;
        }
        public RelayCommand SendCommand { get { return new RelayCommand(SendCommandHandle); } }

        private void SendCommandHandle()
        {
            if (_selected != null)
            {
                Message mess = new Message();
                mess.Content = _selected.CurrentContent;
                mess.IsReceived = false;
                _selected.Messages.Add(mess);
                Out.SendTextMessage(mess);

            }
        }

        #endregion

        #region Method
        private void Client_Received(NetSource sender, NetSourceEventArgs e)
        {
            try
            {
                if (Handler?[e.Package.Code] != null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => Handler[e.Package.Code].Handle(this, e.Package));
                }
            }
            catch (Exception ex)
            {
                messagerService.ShowError(ex, "Exception", "OK", null);
            }
        }
        public void AddUser(UserInfo user)
        {
            try
            {
                OnlineUsers.Add(user);
            }
            catch (Exception ex)
            {
                messagerService.ShowError(ex, "Exception", "OK", null);
            }
        }
        public void AddMessageInfo(Message mess)
        {
            try
            {
               // UserMessage g = _UserMessages.Single((p) => p.User == mess.GroupID);
            }
            catch (Exception ex)
            {
                messagerService.ShowError(ex, "Exception", "OK", null);
            }
        }
        #endregion
        #region Sample Data
        UserMessageCollection GetSampleGroup()
        {
            UserMessageCollection coll = new UserMessageCollection();
            UserMessage g = new UserMessage();
            UserInfo u = new UserInfo();
            u.UserID = 1;
            u.UserName = "DucKhan";
            UserInfo u1 = new UserInfo();
            u1.UserID = 2;
            u1.UserName = "Thanh Long";
            g.User = u1;
            Message info = new Message();
            info.Content = "String message content";
            info.Type = eMessageType.Text;
            info.GroupID = 1;
            g.Messages.Add(info);
            info = new Message();
            info.Content = "ms-appx:///Assets/nao.jpg";
            info.Type = eMessageType.Image;
            info.IsReceived = true;
            info.GroupID = 1;
            g.Messages.Add(info);
            g.CurrentContent = "Current content";
            coll.Add(g);
            return coll;
        }
        #endregion
    }

}
