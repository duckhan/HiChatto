using GalaSoft.MvvmLight.Views;
using HiChatto.Universal.ViewModels;
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
    public sealed partial class StartPage : Page
    {
        static StartPage Current;
        public StartPage()
        {
            this.InitializeComponent();
            DataContext = new StartViewModel();
            Current = this;
        }  
    }
}
