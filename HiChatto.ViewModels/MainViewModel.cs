using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using HiChatto.Models;
using HiChatto.Base.Net;

namespace HiChatto.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private List<UserMessage> userMessages;
        public List<UserMessage> UserMessages
        {
            get { return userMessages; }
            set { userMessages = value; }
        }
        NetSource client;
        public MainViewModel()
        {
            userMessages = GetSampleData();
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
            return models;
        }
    }

}
