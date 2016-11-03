using HiChatto.Universal.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HiChatto.Universal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainView : Page
    {
        public MainView()
        {
  
            this.InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }
        public MainViewModel ViewModel;

    }

}
