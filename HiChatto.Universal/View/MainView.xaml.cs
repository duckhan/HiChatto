using System;
using System.Threading.Tasks;
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

    public sealed partial class MainView : BasePage
    {
        public MainView():base("MainView")
        {

            this.InitializeComponent();
            try
            {
                ViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
            }
            catch
            {
                ViewModel = new MainViewModel(this);
                SimpleIoc.Default.Register(() => ViewModel);
            }
            DataContext = ViewModel;
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
    }

}
