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
           
            Received += Client_Received;
        }

        private void Client_Received(NetSource sender, NetSourceEventArgs e)
        {
            Server.Handlers[e.Package.Code]?.Handle(sender, e.Package);
        }

        private void ReceiveAsyncComplete(object sender, SocketAsyncEventArgs e)
        {
            if (((Socket)sender).Connected && e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                lock (_recieveBuffer)
                {
                    OnRecieve(e.BytesTransferred);
                    ImpReceiveAsync(e);
                }
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
            OnConnect();
        }
        public void ReceiveAsync()
        {
            if (_socket != null)
            {
                _socket.ReceiveAsync(rc_event);
            }
        }
        public override void Disconnect()
        {
            _isConnected = false;
            _socket.Dispose();
            rc_event.Dispose();
            OnDisconnect();
        }

        public override void Send(Package pkg)
        {
            throw new NotImplementedException();
        }



        protected override void SendTCP(int numBytes, Package pkg)
        {
            throw new NotImplementedException();
        }

        public override void Connect()
        {
        }
    }
}
