using HiChatto.Universal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiChatto.Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage(StartViewModel viewModel)
        {
            this.InitializeComponent();
           DataContext = viewModel;
        }
       // public StartViewModel ViewModel { get; set; }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
        }
        string GetDeviceName()
        {
            var info = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
            return info.FriendlyName;
        }
        HostName GetHostName()
        {
            var hostnames = NetworkInformation.GetHostNames();
            foreach (var item in hostnames)
            {
                if (item.Type == HostNameType.Ipv4)
                {
                    return item;
                }
            }
            return null;
        }

    }
}
