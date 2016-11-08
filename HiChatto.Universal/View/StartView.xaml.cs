using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using HiChatto.ViewModels;
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
    public sealed partial class StartView : BasePage
    {
        StartViewModel viewModel;
        public StartView() : base("StartView")
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
    }
}
