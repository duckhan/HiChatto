﻿using HiChatto.Universal.ViewModels.Communicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

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
        public BasePage(string pageName, object parametter)
        {
            Name = pageName;
            Parameter = parametter;
            RegisterPage(pageName, this.GetType());
        }
        public string CurrentPageKey
        {
            get
            {
                return (this).Name;
            }
        }

        public object Parameter { get; private set; }

        public void GoBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        public async void NavigateTo(string pageKey)
        {
            if (pages.ContainsKey(pageKey))
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(pages[pageKey]));
                
            }
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            if (pages.ContainsKey(pageKey))
            {
                Frame.Navigate(pages[pageKey], parameter);
            }
        }

        public async void ShowMessage(string message, string title)
        {

            MessageDialog dialog = new MessageDialog(message, title);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
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

        public void ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }
    }
}