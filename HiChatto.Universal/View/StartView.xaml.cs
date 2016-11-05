using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HiChatto.Universal.ViewModels;
using System;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiChatto.Universal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartView : Page, INavigationService
    {
        StartViewModel viewModel;
        public StartView()
        {

            try
            {
                viewModel = SimpleIoc.Default.GetInstance<StartViewModel>();
            }
            catch (System.Exception)
            {
                viewModel = new StartViewModel(this);
                SimpleIoc.Default.Register<StartViewModel>(() => viewModel);
            }
            finally
            {
                this.InitializeComponent();
                DataContext = viewModel;
            }
        }
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
                Goto(app.PageTypes[pageKey],null);
            }

        }
        async void Goto(Type pageType,object parameter)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Frame.Navigate(pageType,parameter));
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
