using System.Collections.Generic;
using HiChatto.Base.Net;
using HiChatto.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml;

namespace HiChatto.Universal.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private List<UserMessage> userMessages;
        public List<UserMessage> UserMessages
        {
            get { return userMessages; }
            set { userMessages = value; }
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
        NetSource client;
        public MainViewModel()
        {
            _ContentVistable = Visibility.Collapsed;
            userMessages = GetSampleData();
        }

        public RelayCommand ListViewSelectedCommand { get { return new RelayCommand(ListViewSelected); } }

        void ListViewSelected()
        {
            IsMessageContentVisitable = Visibility.Visible;
        }
        List<UserMessage> GetSampleData()
        {
            List<UserMessage> models = new List<UserMessage>();
            UserMessage u = new UserMessage();
            u.User = new UserInfo() { UserID = 1, UserName = "Duc Khan" };
            u.Messages.GroupID = 1;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Duckhan 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Duckhan 2", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Duckhan 3", IsReceived = true });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 2, UserName = "Thanh Long" };
            u.Messages.GroupID = 2;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Thanh Long 1", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Thanh Long 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Thanh Long 3", IsReceived = true });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 3, UserName = "Thanh Duy" };
            u.Messages.GroupID = 3;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Thanh Duy 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Thanh Duy 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Thanh Duy 3", IsReceived = false });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 4, UserName = "Duy Khanh" };
            u.Messages.GroupID = 4;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Duy Khanh 1", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Duy Khanh 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Duy Khanh 3", IsReceived = true });
            models.Add(u);


            u = new UserMessage();
            u.User = new UserInfo() { UserID = 5, UserName = "Tan Phuc" };
            u.Messages.GroupID = 5;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Tan Phuc 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Tan Phuc 2", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Tan Phuc 3", IsReceived = true });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 6, UserName = "Dinh Duong" };
            u.Messages.GroupID = 6;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Dinh Duong 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Dinh Duong 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Dinh Duong 3", IsReceived = false });
            models.Add(u);



            u = new UserMessage();
            u.User = new UserInfo() { UserID = 7, UserName = "Chi Cuong" };
            u.Messages.GroupID = 7;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Chi Cuong 1", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Chi Cuong 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Chi Cuong 3", IsReceived = true });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 8, UserName = "Minh Nhat" };
            u.Messages.GroupID = 8;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Minh Nhat 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Minh Nhat 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Minh Nhat 3", IsReceived = false });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 9, UserName = "Truc Ly" };
            u.Messages.GroupID = 9;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Truc Ly 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Truc Ly 2", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Truc Ly 3", IsReceived = true });
            models.Add(u);




            u = new UserMessage();
            u.User = new UserInfo() { UserID = 10, UserName = "Thai Bao" };
            u.Messages.GroupID = 10;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Thai Bao 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Thai Bao 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Thai Bao 3", IsReceived = false });
            models.Add(u);


            u = new UserMessage();
            u.User = new UserInfo() { UserID = 11, UserName = "Triet Khang" };
            u.Messages.GroupID = 11;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Triet Khang 1", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Triet Khang 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Triet Khang 3", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 4, Content = "Test Message Conent Nguyen Phuc 1", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 5, Content = "Test Message Conent Nguyen Phuc 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 6, Content = "Test Message Conent Nguyen Phuc 3 Test Message Conent Nguyen Phuc 3 Test Message Conent Nguyen Phuc 3 Test Message Conent Nguyen Phuc 3 Test Message Conent Nguyen Phuc 3", IsReceived = true });

            models.Add(u);


            u = new UserMessage();
            u.User = new UserInfo() { UserID = 12, UserName = "Thanh Nam" };
            u.Messages.GroupID = 12;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Thanh Nam 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Thanh Nam 2", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Thanh Nam 3", IsReceived = true });
            models.Add(u);


            u = new UserMessage();
            u.User = new UserInfo() { UserID = 13, UserName = "Thuy Dung" };
            u.Messages.GroupID = 13;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Thuy Dung 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Thuy Dung 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Thuy Dung 3", IsReceived = false });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 14, UserName = "Thuy Kieu" };
            u.Messages.GroupID = 14;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Thuy Kieu 1", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Thuy Kieu 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Thuy Kieu 3", IsReceived = true });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 15, UserName = "Quang Hiep" };
            u.Messages.GroupID = 15;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Quang Hiep 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Quang Hiep 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Quang Hiep 3", IsReceived = false });
            models.Add(u);
            u = new UserMessage();
            u.User = new UserInfo() { UserID = 16, UserName = "Nhat Linh" };
            u.Messages.GroupID = 16;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Nhat Linh 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Nhat Linh 2", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Nhat Linh 3", IsReceived = true });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 17, UserName = "Tien Thanh" };
            u.Messages.GroupID = 17;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Tien Thanh 1", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Tien Thanh 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Tien Thanh 3", IsReceived = true });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 18, UserName = "Nguyen Vu" };
            u.Messages.GroupID = 18;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Nguyen Vu 1", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Nguyen Vu 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Nguyen Vu 3", IsReceived = false });
            models.Add(u);

            u = new UserMessage();
            u.User = new UserInfo() { UserID = 19, UserName = "Nhat Vinh" };
            u.Messages.GroupID = 19;
            u.Messages.Add(new MessageInfo() { ID = 1, Content = "Test Message Conent Nhat Vinh 1", IsReceived = false });
            u.Messages.Add(new MessageInfo() { ID = 2, Content = "Test Message Conent Nhat Vinh 2", IsReceived = true });
            u.Messages.Add(new MessageInfo() { ID = 3, Content = "Test Message Conent Nhat Vinh 3", IsReceived = true });
            models.Add(u);

            return models;
        }


    }

}
