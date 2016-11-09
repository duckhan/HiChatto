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
using System.Net.Http;
using HiChatto.Base.Net.Transfer;

namespace HiChatto.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        #region Fields/Properties

        static readonly string RemoteHost = "http://192.168.137.1:8888/api/upload/";
        static readonly string TextToastNotificationPattern= "<toast><visual><binding template=\"ToastGeneric\"><text>{0}</text></binding></visual></toast>";
        static readonly string SentSoundEffect = "ms-appx:///Assets/sound/sent.mp3";
        static readonly string ReceivedSoundEffect = "ms-appx:///Assets/sound/received.mp3";
        IUploader _uploader;
        public void SetUploader(IUploader uploader)
        {
            _uploader = uploader;
            _uploader.SetToastNotification(string.Format(TextToastNotificationPattern, "Upload was successful."));
        }

        private string _EffectSound;
        public string EffectSound
        {
            get
            {
                return _EffectSound;
            }
            set
            {
                _EffectSound = value;
                RaisePropertyChanged("EffectSound");
            }
        }

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

        public RelayCommand<List<string>> SendAttachCommand
        {
            get
            {
                return new RelayCommand<List<string>>(SendAttachHandle);
            }
        }
        private async void SendAttachHandle(List<string> files)
        {
            if (_uploader != null)
            {
                try
                {
                    ResponseInfo res = await _uploader.UploadAsync(RemoteHost, files.ToArray());
                    UploadCompleted(res);
                }
                catch (Exception ex)
                {
                    messagerService.ShowError(ex, "Error", "OK", null);
                }
            }
        }

        public RelayCommand<UserMessage> ListViewItemSelected { get { return new RelayCommand<UserMessage>(ItemSelectedHandle); } }
        private void ItemSelectedHandle(UserMessage g)
        {
            _selected = g;
            _selected.UnReadCount = 0;
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
                EffectSound = SentSoundEffect;
            }
        }

        public RelayCommand<List<string>> SendImageCommand
        {
            get { return new RelayCommand<List<string>>(SendImageHandle); }
        }

        private async void SendImageHandle(List<string> imgs)
        {
            try
            {
                var res = await _uploader.UploadAsync(RemoteHost, imgs.ToArray(),false);
                UploadImageCompleted(res);
            }
            catch (Exception ex)
            {
                messagerService.ShowError(ex, "Exception", "OK", null);
            }
        }

        public RelayCommand<StickyInfo> SickySelectedCommand
        {
            get { return new RelayCommand<StickyInfo>(SendStickyHandler); }
        }
        private void SendStickyHandler(StickyInfo stick)
        {
            try
            {
                Message mess = new Message();
                mess.IDSender = User.UserID;
                mess.IDReceiver = _selected.User.UserID;
                mess.Type = eMessageType.Sticky;
                mess.Content = stick.FilePath;
                Out.SendTextMessage(mess);
                _selected.Messages.Add(mess);
                EffectSound = SentSoundEffect;
            }
            catch (Exception ex)
            {
                messagerService.ShowError(ex, "Exception", "OK", null);
            }           
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
        public void AddUser(UserInfo user,bool isNewUser=false)
        {
            try
            {
                UserMessage u = new UserMessage(user);
                UserMessages.Add(u);
                if (isNewUser)
                {
                    string content = string.Format("{0} is online now.", u.User.UserName);
                    messagerService.PushToastNotification(string.Format(TextToastNotificationPattern, content));
                }
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
                if (_selected==null || g.User.UserID != _selected.User.UserID)
                {
                    g.UnReadCount++;               
                }
                g.AddMessage(mess);
                EffectSound = ReceivedSoundEffect;
            }
            catch (Exception ex)
            {
                messagerService.ShowError(ex, "Exception", "OK", null);
            }
        }

        private void UploadCompleted(ResponseInfo res)
        {
            try
            {
                if (res.StatusCode == 200)
                {
                    Message mess = new Message(eMessageType.File);
                    mess.IDSender = User.UserID;
                    mess.IDReceiver = _selected.User.UserID;
                    mess.Content = res.Response;
                    Out.SendTextMessage(mess);
                    _selected.Messages.Add(mess);
                    EffectSound = SentSoundEffect;
                }
                else
                {
                    messagerService.ShowMessage(res.Response, "Response");
                }
            }
            catch (Exception ex)
            {
                messagerService.ShowError(ex, "Error", "OK", null);
            }
        }
        private void UploadImageCompleted(ResponseInfo res)
        {
            try
            {
                if (res.StatusCode == 200)
                {
                    string[] imgs = res.Response.Split('\n');
                    foreach (var item in imgs)
                    {
                        Message mess = new Message(eMessageType.Image);
                        mess.IDSender = User.UserID;
                        mess.IDReceiver = _selected.User.UserID;
                        mess.Content = res.Response;
                        Out.SendTextMessage(mess);
                        _selected.Messages.Add(mess);
                    }
                    EffectSound = SentSoundEffect;
                }
                else
                {
                    messagerService.ShowMessage(res.Response, "Response");
                }
            }
            catch (Exception ex)
            {
                messagerService.ShowError(ex, "Error", "OK", null);
            }
        }
        #endregion
        #region Sample Data
        UserMessageCollection GetSampleGroup()
        {
            UserMessageCollection coll = new UserMessageCollection();
            UserMessage g = new UserMessage();
            g.UnReadCount = 4;
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
