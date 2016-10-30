using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace HiChatto.Server
{
    public class Server
    {
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
            _clients = new List<Client>();
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _ipe = new IPEndPoint(IPAddress.Parse(_config.IP), _config.Port);
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
            catch(Exception ex)
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
                _clients.Add(c);
                c.Connect(e.AcceptSocket);
                c.Disconnected += Client_Disconnected;
                c.ReceiveAsync();
            }
            catch(Exception ex)
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
            Console.WriteLine("Disconnected {0}", c.Socket.RemoteEndPoint.ToString());
            _clients.Remove(c);
        }
    }
}
