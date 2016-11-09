using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HiChatto.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using System.Threading;
using HiChatto.Universal.Net;
using HiChatto.Base.Net;
using HiChatto.Models;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using HiChatto.Universal.Net.Transfer;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiChatto.Universal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainView : BasePage
    {

        public MainView() : base("MainView")
        {
            UniversalClient client = (App.Current as App).Client;
            this.InitializeComponent();
            FileUploader uploader = new FileUploader();
            ViewModel = new MainViewModel(this, new PackageOut(client), client, SynchronizationContext.Current);
            ViewModel.SetUploader(uploader);
            DataContext = ViewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        public MainViewModel ViewModel;

        private void txtMessage_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (ViewModel.SendCommand.CanExecute(null))
                {
                    ViewModel.SendCommand.Execute(null);
                }

            }
        }

        private void HambergerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            RootSplitView.IsPaneOpen = !RootSplitView.IsPaneOpen;
        }

        private async void AttachButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add("*");
            var files = await filePicker.PickMultipleFilesAsync();
            List<string> filePaths = new List<string>();
            if (files != null && files.Count > 0)
            {
                foreach (var item in files)
                {
                    string token = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(item);
                    filePaths.Add(item.Path);
                }
                if (ViewModel.SendAttachCommand.CanExecute(null))
                {
                    ViewModel.SendAttachCommand.Execute(filePaths);
                }
            }
        }

        private async void PictureButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".bmp");
            var files = await filePicker.PickMultipleFilesAsync();
            List<string> filePaths = new List<string>();
            if (files != null && files.Count > 0)
            {
                foreach (var item in files)
                {
                    string token = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(item);
                    filePaths.Add(item.Path);
                }
                if (ViewModel.SendImageCommand.CanExecute(null))
                {
                    ViewModel.SendImageCommand.Execute(filePaths);
                }
            }
        }
    }

}
