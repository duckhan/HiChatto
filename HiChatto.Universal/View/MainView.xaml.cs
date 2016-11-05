﻿using System;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HiChatto.Universal.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiChatto.Universal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainView : Page, INavigationService
    {
        public MainView()
        {

            this.InitializeComponent();
            try
            {
                ViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
            }
            catch
            {
                ViewModel = new MainViewModel(this);
                SimpleIoc.Default.Register<MainViewModel>(() => ViewModel);
            }
            DataContext = ViewModel;
        }
        public MainViewModel ViewModel;
        public string CurrentPageKey
        {
            get
            {
                return "MainView";
            }
        }
        public void GoBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        public void NavigateTo(string pageKey)
        {
            App app = (App)App.Current;
            if (app.PageTypes.ContainsKey(pageKey))
            {
                Goto(app.PageTypes[pageKey], null);
            }

        }
        async void Goto(Type pageType, object parameter)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Frame.Navigate(pageType, parameter));
        }
        public void NavigateTo(string pageKey, object parameter)
        {
            App app = (App)App.Current;
            if (app.PageTypes.ContainsKey(pageKey))
            {
                Goto(app.PageTypes[pageKey], parameter);
            }
        }
    }

}
