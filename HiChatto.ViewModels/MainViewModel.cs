using HiChatto.Base.Net;
using HiChatto.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using System;
using HiChatto.ViewModels.Communicate;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace HiChatto.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        #region Fields/Properties
        SynchronizationContext _context;

        private List<StickyInfo> _Stickies;

        public List<StickyInfo> Stickies
        {
            get { return _Stickies; }
            set { _Stickies = value; }
        }


        private bool _IsLoading = false;
        private IPackageHandler[] _handler;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                _context.Post(new SendOrPostCallback((o) => RaisePropertyChanged("IsLoading")), this);
            }
        }
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
            get { return _handler; }
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
        private NetSource client;
        private IPackageOut Out;

        #endregion

        #region Contructor
        public MainViewModel(IMessagerSercive service, IPackageOut pkgOut, NetSource netSource, SynchronizationContext context)
        {
            _context = context;
            IsLoading = true;
            this.messagerService = service;

            _Stickies = LoadAllSitcky();
            _handler = SimpleIoc.Default.GetInstance<IPackageHandler[]>();
            _UserMessages = new UserMessageCollection();
#if DEBUG
            _UserMessages = GetSampleGroup();
#endif
            _OnlineUsers = new UserCollection();
            _ContentVistable = false;
            client = netSource;
            client.Received += Client_Received;
            client.ReceiveAsync();
            client.Disconnected += Client_Disconnected;
            Out = pkgOut;
            User.UserName = client.Config.UserName;
            Out.SendUserConnect();
            Task.Delay(2000).ContinueWith((task) =>
            {
                Out.SendGetAllUser();
            }).ContinueWith((t) => IsLoading = false);
        }

        private void Client_Disconnected(NetSource sender, NetSourceEventArgs e)
        {
            messagerService.ShowMessage("Disconnected", "Network Infomation");
            SimpleIoc.Default.Unregister<NetSource>();
            messagerService.GoBack();
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
                mess.IDReceiver = _selected.User.UserID;
                mess.IDSender = User.UserID;
                mess.IsReceived = false;
                _selected.CurrentContent = "";
                _selected.Messages.Add(mess);
                Out.SendTextMessage(mess);

            }
        }

        public RelayCommand<StickyInfo> SickySelectedCommand
        {
            get { return new RelayCommand<StickyInfo>(SendStickyHandler); }
        }
        private void SendStickyHandler(StickyInfo stick)
        {
            Message mess = new Message();
            mess.IDSender = User.UserID;
            mess.IDReceiver = _selected.User.UserID;
            mess.Type = eMessageType.Sticky;
            mess.Content = stick.FilePath;
            Out.SendTextMessage(mess);
            _selected.Messages.Add(mess);
        }


        #endregion

        #region Method

        public List<StickyInfo> LoadAllSitcky()
        {
            List<StickyInfo> list = new List<StickyInfo>();
            for (int i = 1; i <= 20; i++)
            {
                list.Add(new StickyInfo() { ID = i, FilePath = string.Format("ms-appx:///Assets/Sticky/chat{0}.png", i) });
            }
            return list;
        }

        public void SetHandlers(IPackageHandler[] handlers)
        {
            _handler = handlers;
        }
        private void Client_Received(NetSource sender, NetSourceEventArgs e)
        {
            try
            {
                if (Handler?[e.Package.Code] != null)
                {
                    _context.Post((o) => Handler[e.Package.Code].Handle(this, e.Package), this);
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
                UserMessage u = new UserMessage(user);
                UserMessages.Add(u);
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
                UserMessage g = _UserMessages.Single(u => u.User.UserID == mess.IDSender);
                g.AddMessage(mess);
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
            info.IDSender = 1;
            g.Messages.Add(info);
            info = new Message();
            info.Content = "ms-appx:///Assets/nao.jpg";
            info.Type = eMessageType.Image;
            info.IsReceived = true;
            info.IDSender = 1;
            g.Messages.Add(info);
            g.CurrentContent = "Current content";
            coll.Add(g);
            return coll;
        }
        #endregion
    }

}
