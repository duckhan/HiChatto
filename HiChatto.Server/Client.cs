using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using HiChatto.Base;
using HiChatto.Base.Net;
using System.Reflection;
using System.Threading;

namespace HiChatto.Server
{
    public class Client : NetSource
    {
        Socket _socket;
        public Socket Socket
        {
            get { return _socket; }
        }
        public int id;
        SocketAsyncEventArgs rc_event;
        public Client() : base(new byte[8096], new byte[8096])
        {
            id = 0;
            rc_event = new SocketAsyncEventArgs();
            rc_event.SetBuffer(_recieveBuffer, 0, 8096);
            rc_event.Completed += ReceiveAsyncComplete;
            if (_handlers == null)
            {
                LoadPackageHandler();
            }
        }

        private void ReceiveAsyncComplete(object sender, SocketAsyncEventArgs e)
        {
            if (((Socket)sender).Connected && e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                byte[] buff = new byte[e.BytesTransferred];
                Array.Copy(e.Buffer, buff, buff.Length);
                Console.WriteLine("Recive From {0}:", _socket.RemoteEndPoint.ToString());
                Console.Write(Helper.ToHexDump(buff));
                string str = UnicodeEncoding.UTF8.GetString(buff);
                Console.WriteLine(str);
                byte[] send = UnicodeEncoding.UTF8.GetBytes("Server: " + str);
                byte[] leng = BitConverter.GetBytes(send.Length);
                byte[] pack = new byte[leng.Length + send.Length];
                Array.Copy(leng, pack, leng.Length);
                Array.Copy(send, 0, pack, leng.Length, send.Length);
                Send(pack);
                 send = UnicodeEncoding.UTF8.GetBytes("Server 2: " + str);
                 leng = BitConverter.GetBytes(send.Length);
                 pack = new byte[leng.Length + send.Length];
                Array.Copy(leng, pack, leng.Length);
                Array.Copy(send, 0, pack, leng.Length, send.Length);
                Thread.Sleep(10000);
                Send(pack);
                ImpReceiveAsync(e);
            }
            else
            {
                Disconnect();
            }
        }
        private void Send(byte[] buff)
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(buff, 0, buff.Length);
            e.Completed += SendAsyncCompleted;
            e.RemoteEndPoint = _socket.RemoteEndPoint;
            _socket.SendAsync(e);
        }

        private void SendAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine("Sent {0} bytes to {1}", e.BytesTransferred, e.RemoteEndPoint.ToString());
        }

        private void ImpReceiveAsync(SocketAsyncEventArgs e)
        {
            if (_socket != null && _socket.Connected)
            {
                e.SetBuffer(_recieveBuffer, 0, _recieveBuffer.Length);
                _socket.ReceiveAsync(e);
            }
        }

        public void Connect(Socket sk)
        {
            id = Server.Clients.Count + 1;
            _socket = sk;
            if (_connected != null)
            {
                _connected(this, EventArgs.Empty);
            }
        }
        public void ReceiveAsync()
        {
            if (_socket != null)
            {
                _socket.ReceiveAsync(rc_event);
            }
        }
        private void Disconnect()
        {
            if (_disconnected != null)
            {
                _disconnected(this, EventArgs.Empty);
            }
            _socket.Dispose();
        }

        public override void Send(Package pkg)
        {
            throw new NotImplementedException();
        }

        protected override void LoadPackageHandler()
        {
            _handlers = new IPackageHandler[64];
            var assembly = Assembly.GetAssembly(typeof(HiChatto.Server.Client));
            var types = assembly.GetTypes();
            foreach (var item in types)
            {
                if (item.IsClass && item.GetInterface("HiChatto.Base.Net.IPackageHandler") != null)
                {
                    PackageHandlerAttribute a = (PackageHandlerAttribute)item.GetCustomAttribute(typeof(PackageHandlerAttribute));
                    _handlers[0] = (IPackageHandler)Activator.CreateInstance(item);
                    Console.WriteLine("Found");

                }
            }
        }
    }
}
