using HiChatto.Base.Net;
using HiChatto.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Views;
using HiChatto.Universal.Net;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System;
using HiChatto.Universal.ViewModels.Communicate;

namespace HiChatto.Universal.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields/Properties
        private ObservableCollection<GroupMessage> groups;
        public ObservableCollection<GroupMessage> Groups
        {
            get { return groups; }
            set
            {
                Set("Groups", ref groups, value);
            }
        }
        private GroupMessage _selectedGroup;
        public GroupMessage SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                Set("SelectedGroup", ref _selectedGroup, value);
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
        private Visibility _ContentVistable;
        public Visibility IsMessageContentVisitable
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
            _ContentVistable = Visibility.Collapsed;
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
        public RelayCommand<GroupMessage> ListViewItemSelected { get { return new RelayCommand<GroupMessage>(ItemSelectedHandle); } }
        private void ItemSelectedHandle(GroupMessage g)
        {
            if (g != null)
            {
                if (_selectedGroup == null || _selectedGroup.GroupID != g.GroupID)
                {
                    _selectedGroup = g;
                }
            }
            IsMessageContentVisitable = Visibility.Visible;
        }
        public RelayCommand SendCommand { get { return new RelayCommand(SendCommandHandle); } }

        private void SendCommandHandle()
        {
            if (_selectedGroup != null)
            {
                MessageInfo mess = new MessageInfo();
                mess.Content = SelectedGroup.CurrentContent;
                mess.IsReceived = false;
                _selectedGroup.Messages.Add(mess);
                Out.SendTextMessage(mess);
                SelectedGroup.CurrentContent = "";

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
                    Handler[e.Package.Code].Handle(this, e.Package);
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
        public void AddMessageInfo(MessageInfo mess)
        {
            try
            {
                GroupMessage g = groups.Single((p) => p.GroupID == mess.GroupID);
            }
            catch (Exception ex)
            {

                messagerService.ShowError(ex, "Exception", "OK", null);
            }
        }
        #endregion

    }

}
