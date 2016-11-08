using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using HiChatto.Base.Net;
using System.Reflection;

namespace HiChatto.Server
{
    public class Server
    {
        static IPackageHandler[] _handlers;
        public static IPackageHandler[] Handlers
        {
            get { return _handlers; }
        }
        private ServerConfig _config;
        public ServerConfig Config
        {
            get { return _config; }
        }
        Socket _listener;
        const int PORT = 6969;
        IPEndPoint _ipe;
        static List<Client> _clients;
        public static List<Client> Clients
        {
            get { return _clients; }
        }
        public Server()
        {
            _config = new ServerConfig();
        }
        public void Start()
        {
            if (_handlers == null)
            {
                LoadPackageHandler();
            }
            _clients = new List<Client>();
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // _ipe = new IPEndPoint(IPAddress.Parse(_config.IP), _config.Port);
            _ipe = new IPEndPoint(IPAddress.Any, _config.Port);
            _listener.Bind(_ipe);
            _listener.Listen(100);
            Listen();
        }
        private void Listen()
        {
            Console.WriteLine("Server is listening at {0}", _ipe.ToString());
            if (_listener != null)
            {
                AcceptAsync();
            }
        }
        private void AcceptAsync()
        {
            try
            {
                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.Completed += AcceptAsyncComplete;
                _listener.AcceptAsync(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }
        }
        private void AcceptAsyncComplete(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                Console.WriteLine("Accept conect from: {0}", e.AcceptSocket.RemoteEndPoint.ToString());
                Client c = new Client();
                c.Connect(e.AcceptSocket);
                c.Disconnected += Client_Disconnected;
                _clients.Add(c);
                c.ReceiveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }
            finally
            {
                e.Dispose();
                AcceptAsync();
            }
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            var c = (Client)sender;
            int UserId = c.User.UserID;

            _clients.Remove(c);
            Console.WriteLine("Disconnted: " + c.User.UserName);

            foreach (var item in _clients)
            {

            }
        }
        protected void LoadPackageHandler()
        {
            _handlers = new IPackageHandler[64];
            var assembly = Assembly.GetAssembly(typeof(HiChatto.Server.Client));
            var types = assembly.GetTypes();
            foreach (var item in types)
            {
                if (item.IsClass && item.GetInterface("HiChatto.Base.Net.IPackageHandler") != null)
                {
                    PackageHandlerAttribute a = (PackageHandlerAttribute)item.GetCustomAttribute(typeof(PackageHandlerAttribute));
                    _handlers[a.Code] = (IPackageHandler)Activator.CreateInstance(item);
                    Console.WriteLine("Loaded {0} Handler", _handlers[a.Code].ToString());

                }
            }
        }

        public static Client GetClient(int clientID)
        {
            foreach (var item in _clients)
            {
                if (item.User.UserID == clientID)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
