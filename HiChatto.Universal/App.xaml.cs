using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HiChatto.Universal.View;
using HiChatto.Universal.Net;
using GalaSoft.MvvmLight.Ioc;
using HiChatto.Models;
using HiChatto.ViewModels;
using HiChatto.ViewModels.Communicate;
using GalaSoft.MvvmLight.Threading;
using HiChatto.Base.Net;
using System.Reflection;
using System.Reflection.Metadata;
using Windows.Storage;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HiChatto.Universal
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            RegisterPageTypes();
            LoadPackageHandlers();
        }
        private Dictionary<string, Type> pageTypes;
        public Dictionary<string, Type> PageTypes
        {
            get { return pageTypes; }
        }
        private IPackageHandler[] handlers;
        private ClientConfig _config;
        public ClientConfig Config
        {
            get { return _config; }
        }
        private UniversalClient _Client;
        public UniversalClient Client
        {
            get
            {
                return _Client;
            }
            set
            {
                _Client = value;
            }
        }
        public IPackageHandler[] Handlers { get { return handlers; } }
        void LoadPackageHandlers()
        {
            handlers = new IPackageHandler[64];
            Assembly assembly = Assembly.Load(new AssemblyName("HiChatto.Universal"));
            foreach (Type item in assembly.ExportedTypes)
            {
                Type[] interfaces = item.GetInterfaces();
                if (interfaces.Length > 0 && interfaces[0] == typeof(IPackageHandler))
                {
                    CustomAttributeData a = item.GetTypeInfo().CustomAttributes.ToArray()?[0];
                    if (a != null)
                    {
                        int code = (int)a.ConstructorArguments[0].Value;
                        handlers[code] = (IPackageHandler)Activator.CreateInstance(item);
                    }
                }
            }
        }
        private async void SaveConfigAsync(ClientConfig _config)
        {
            var local = ApplicationData.Current.LocalFolder;
            var file = await local.CreateFileAsync("setting.json", CreationCollisionOption.ReplaceExisting);
            string str = JsonConvert.SerializeObject(_config);

            File.WriteAllText(file.Path, str);
        }
        private async Task LoadConfigAsync()
        {
            try
            {
                var local = ApplicationData.Current.LocalFolder; ;
                var file = await local.GetFileAsync("setting.json");
                if (file.IsAvailable)
                {
                    string str = File.ReadAllText(file.Path);
                    _config = JsonConvert.DeserializeObject<ClientConfig>(str);
                    SimpleIoc.Default.Register<ClientConfig>(() => _config);
                }
            }
            catch
            {
                _config = new ClientConfig();
            }
        }
        void RegisterPageTypes()
        {
            pageTypes = new Dictionary<string, Type>();
            pageTypes.Add("MainView", typeof(MainView));
            pageTypes.Add("StartView", typeof(StartView));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            await LoadConfigAsync();
            if (!SimpleIoc.Default.IsRegistered<IPackageHandler[]>())
            {
                SimpleIoc.Default.Register(() => handlers);
            }
            Action<ClientConfig> saveAction = SaveConfigAsync;
           if (!SimpleIoc.Default.IsRegistered<Action<ClientConfig>>())
            {
                SimpleIoc.Default.Register(() => saveAction);
            }
            DispatcherHelper.Initialize();
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            SimpleIoc.Default.Register<Frame>(() => rootFrame);
            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(StartView), e.Arguments);
                    // IFrameNavigationService navigate = SimpleIoc.Default.GetInstance<IFrameNavigationService>();
                    //navigate.NavigateTo("StartView");
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            // SaveConfigAsync(_config);
            SimpleIoc.Default.Unregister<ClientConfig>();
            SimpleIoc.Default.Unregister<Action<ClientConfig>>();
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
