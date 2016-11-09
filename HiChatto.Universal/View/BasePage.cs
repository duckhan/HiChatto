using HiChatto.ViewModels.Communicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace HiChatto.Universal.View
{
    public class BasePage : Page, IMessagerSercive
    {
        static Dictionary<string, Type> pages;
        static void RegisterPage(string pageName, Type p)
        {
            lock (pages)
            {
                if (!pages.ContainsKey(pageName))
                {
                    pages.Add(pageName, p);
                }
            }
        }
        static BasePage()
        {
            if (pages == null)
            {
                pages = new Dictionary<string, Type>();
            }
            RegisterPage("MainView", typeof(MainView));
            RegisterPage("StartView", typeof(StartView));
        }
        public BasePage(string pageName)
        {

            Name = pageName;
            RegisterPage(pageName, this.GetType());
        }

        public BasePage(string pageName, object parametter) : this(pageName)
        {
            Parameter = parametter;
        }
        public string CurrentPageKey
        {
            get
            {
                return (this).Name;
            }
        }

        public object Parameter { get; private set; }

        public async void GoBack()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            });
        }

        public async void NavigateTo(string pageKey)
        {
            if (pages.ContainsKey(pageKey))
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(pages[pageKey]));

            }
        }

        public async void NavigateTo(string pageKey, object parameter)
        {
            if (pages.ContainsKey(pageKey))
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(pages[pageKey], parameter));
            }
        }

        public async void ShowMessage(string message, string title)
        {

            MessageDialog dialog = new MessageDialog(message, title);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }

        public async void ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            dialog.DefaultCommandIndex = 0;
            UICommand com = new UICommand();
            com.Label = buttonText;
            com.Id = 0;
            dialog.Commands.Add(com);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
            afterHideCallback?.Invoke();
        }

        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            dialog.DefaultCommandIndex = 0;
            UICommand com = new UICommand();
            com.Label = buttonConfirmText;
            com.Id = 0;
            dialog.Commands.Add(com);
            com = new UICommand();
            com.Label = buttonCancelText;
            com.Id = 1;
            dialog.Commands.Add(com);
            IUICommand ret = null;
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => ret = await dialog.ShowAsync());
            return ret != null && ret.Label.Equals(buttonConfirmText);

        }

        public async void ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            MessageDialog dialog = new MessageDialog(error.StackTrace, title);
            dialog.Commands.Add(new UICommand() { Label = buttonText });
            IUICommand ret = null;
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => ret = await dialog.ShowAsync());
            afterHideCallback?.Invoke();
        }

        public void PushToastNotification(string xmlTemplate)
        {
            var notifyManager = ToastNotificationManager.CreateToastNotifier();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlTemplate);
            ToastNotification toast = new ToastNotification(xml);
            notifyManager.Show(toast);
        }
    }
}
