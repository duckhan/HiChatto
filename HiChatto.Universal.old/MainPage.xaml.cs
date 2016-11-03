using HiChatto.ViewModels;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiChatto.Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static MainPage Current;
        public MainPage(StartViewModel viewModel)
        {
            this.InitializeComponent();
           DataContext = viewModel;
            Current = this;
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
