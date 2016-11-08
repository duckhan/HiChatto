using HiChatto.Base.Net;
using System;
using System.Net.Sockets;
using System.Net;
using HiChatto.Models;
using System.IO;

namespace HiChatto.Universal.Net
{
    public class UniversalClient : NetSource
    {
        Socket _socket;

        public override bool IsConnected
        {
            get
            {
                return _socket != null && _socket.Connected;
            }

            set
            {
                base.IsConnected = value;
            }
        }
        public Socket Socket { get { return _socket; } }
        SocketAsyncEventArgs receiveEvent;
        public UniversalClient(ClientConfig config) : base(new byte[8096], new byte[8096])
        {
            _config = config;
            _User = new UserInfo();
            _User.UserName = _config.UserName;
        }

        private void ReceiveAsynCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                OnRecieve(e.BytesTransferred);
                ReceiveAsync();
            }
            else
            {
                Disconnect();
            }
        }

        private void ConnectAsyncComplete(object sender, SocketAsyncEventArgs e)
        {
            _isConnected = true;
            OnConnect();
        }

        public override void ReceiveAsync()
        {
            try
            {
                _socket.ReceiveAsync(receiveEvent);
            }
            catch
            {
                Disconnect();
            }
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
                else
                {
                    Disconnect();
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

        public override void Disconnect()
        {

            _isConnected = false;
            _socket?.Dispose();
            receiveEvent?.Dispose();
            OnDisconnect();
        }

        public override void Connect()
        {
            try
            {
                SocketAsyncEventArgs conEv = new SocketAsyncEventArgs();
                receiveEvent = new SocketAsyncEventArgs();
                receiveEvent.Completed += ReceiveAsynCompleted;
                receiveEvent.SetBuffer(_receiveBuffer, 0, _receiveBuffer.Length);
                conEv.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(_config.ServerIP), _config.ServerPort);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                conEv.Completed += ConnectAsyncComplete;
                _socket.ConnectAsync(conEv);

            }
            catch
            {
                _isConnected = false;
            }
        }

    }
}
