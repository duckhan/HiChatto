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
using HiChatto.Models;

namespace HiChatto.Server
{
    public class Client : NetSource
    {
        Socket _socket;
        object obj = new object();

        public Socket Socket
        {
            get { return _socket; }
        }
        public int ID
        {
            get { return _User == null ? 0 : _User.UserID; }
            private set { }
        }
        SocketAsyncEventArgs rc_event;
        public Client() : base(new byte[8096], new byte[8096])
        {
            ID = 0;
            rc_event = new SocketAsyncEventArgs();
            rc_event.SetBuffer(_receiveBuffer, 0, 8096);
            rc_event.Completed += ReceiveAsyncComplete;
            Received += Client_Received;
        }

        private void Client_Received(NetSource sender, NetSourceEventArgs e)
        {

            Console.WriteLine("Receicve Package: {0}, Legnth={1}", e.Package.Code, e.Package.Length);
            Server.Handlers[e.Package.Code]?.Handle(sender, e.Package);
        }

        private void ReceiveAsyncComplete(object sender, SocketAsyncEventArgs e)
        {
            if (((Socket)sender).Connected && e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                lock (_receiveBuffer)
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
        private void ImpReceiveAsync(SocketAsyncEventArgs e)
        {
            if (_socket != null && _socket.Connected)
            {
                e.SetBuffer(_receiveBuffer, 0, _receiveBuffer.Length);
                _socket.ReceiveAsync(e);
            }
        }

        public void Connect(Socket sk)
        {
            //  ID = Server.Clients.Count + 1;
            _socket = sk;
            OnConnect();
        }
        public override void ReceiveAsync()
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


        protected override void SendTCP(int numBytes, Package pkg)
        {
            try
            {
                if (_socket != null && _socket.Connected)
                {
                    lock (_pkgQueue)
                    {
                        _pkgQueue.Enqueue(pkg);
                        if (_isSending)
                        {
                            return;
                        }
                        else
                        {
                            _isSending = true;
                            pkg = _pkgQueue.Dequeue();
                        }
                        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                        e.SetBuffer(_sendBuffer, 0, numBytes);
                        e.UserToken = pkg;
                        e.Completed += SendAsyncCompleted;
                        _socket.SendAsync(e);
                    }

                }
            }
            catch
            {
                Disconnect();
            }
        }
        private void SendAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            NetSourceEventArgs sentEvent = new NetSourceEventArgs();
            sentEvent.UserToken = e;
            OnSent(this, sentEvent);
            _isSending = false;
            if (_pkgQueue.Count > 0)
            {
                Package pkg = _pkgQueue.Dequeue();
                SendTCP(pkg.Length + 4, pkg);
            }
        }

        public override void Connect()
        {
        }
    }
}
